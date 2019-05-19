using Point.Contracts.Models;
using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Services.ImageService.Model;
using Point.Services.InfoPostService.Model;
using Point.Services.RegisterService.Model;
using Point.Services.ReturnImageService.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Point.Services.InstitutionService.Model;

namespace Point.Services.InfoPostService
{
    public class InfoPostService : IInfoPostService
    {
        private readonly bool checkForInstitution = false;
        private readonly IInfoPostRepository _infoPostRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IVotePostRepository _votePostRepository;

        private readonly IRegisterService _registerService;
        private readonly IImageService _imageService;
        private readonly IInstitutionService _institutionService;
        
        public InfoPostService(IInfoPostRepository infoPostRepository, IImageRepository imageRepository, IVotePostRepository votePostRepository,
            IRegisterService registerService, IImageService imageService, IStorageImageService storageImageService, IInstitutionService institutionService)
        {
            _infoPostRepository = infoPostRepository;
            _votePostRepository = votePostRepository;
            _imageRepository = imageRepository;

            _registerService = registerService;
            _imageService = imageService;
            _institutionService = institutionService;
        }

        public async Task AddAsync(Contracts.Models.InfoPost info)
        {
            //var user = await _registerService.GetUserByKeyAsync(info.UserId);

            List<Image> images = new List<Image>();

            foreach (var image in info.Images)
            {
                images.Add(await _imageService.CreateImageAsync(image, info.UserId));
            }

            long? belongToId = null;
            if (checkForInstitution)
            {
                var zones = await _institutionService.GetAllZonesAsync();

                foreach (var zone in zones)
                {
                    var isIn1 = IsPointInPolygon(zone.Points.Select(x => new Loc(x.Latitude, x.Longitude)).ToList(), new Loc(info.Latitude, info.Longitude));
                    if (isIn1)
                    {
                        belongToId = zone.InstitutionId;
                        break;
                    }
                }
            }

            await _infoPostRepository.AddAsync(new Data.Models.InfoPost()
            {
                Description = info.Description,
                Latitude = Convert.ToDecimal(info.Latitude),
                Longitude = Convert.ToDecimal(info.Longitude),
                UserId = info.UserId,
                Image = images,
                VotePost = new List<VotePost>() { new VotePost() { UserId = info.UserId, Vote = true } },
                BelongTo = belongToId
            });
        }

        public async Task<PointDetail> GetAsync(string serverUrl, long postId, long userId)
        {
            var info = await _infoPostRepository.GetAsync(x => x.Id == postId, c => c.User, c => c.VotePost, c => c.BelongToNavigation);

            return await GetPointDetail(serverUrl, info, userId);
        }

        public async Task<PointVotes> GetVotesAsync(long postId, long userId)
        {
            var votes = await _votePostRepository.GetAllAsync(x => x.InfoPostId == postId);

            return GetPointVotes(votes, userId);
        }

        bool IsPointInPolygon(List<Loc> poly, Loc point)
        {
            int i, j;
            bool c = false;
            for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                if ((((poly[i].Lt <= point.Lt) && (point.Lt < poly[j].Lt))
                        || ((poly[j].Lt <= point.Lt) && (point.Lt < poly[i].Lt)))
                        && (point.Lg < (poly[j].Lg - poly[i].Lg) * (point.Lt - poly[i].Lt)
                            / (poly[j].Lt - poly[i].Lt) + poly[i].Lg))
                {
                    c = !c;
                }
            }

            return c;
        }

        private static bool IsPointInPolygon2(List<Loc> zonePoints, Loc point)
        {
            return IsPointInPolygon2(zonePoints, point.Lt, point.Lg);
        }

        private static bool IsPointInPolygon2(List<Loc> zonePoints, double x, double y)
        {
            int polysides = zonePoints.Count();

            //latitude/polyX = horizontal point
            List<double> polyX = new List<double>();
            //logitude/polyY = vertical point
            List<double> polyY = new List<double>();

            try
            {
                polyX = (from z in zonePoints
                         select z.Lt).ToList();


                polyY = (from z in zonePoints
                         select z.Lg).ToList();
            }
            catch (ArgumentNullException) { return false; }
            catch (FormatException) { return false; }
            catch (OverflowException) { return false; }


            int j = polysides - 1;
            bool oddNodes = false;

            for (int i = 0; i < polysides; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y) || (polyY[j] < y && polyY[i] >= y))
                {
                    if (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x)
                        oddNodes = !oddNodes;
                }

                j = i;
            }

            return oddNodes;
        }

        public class Loc
        {
            private double lt;
            private double lg;

            public double Lg
            {
                get { return lg; }
                set { lg = value; }
            }

            public double Lt
            {
                get { return lt; }
                set { lt = value; }
            }

            public Loc(decimal lt, decimal lg)
            {
                this.lt = Convert.ToDouble(lt);
                this.lg = Convert.ToDouble(lg);
            }

            public Loc(double lt, double lg)
            {
                this.lt = lt;
                this.lg = lg;
            }

            public Loc(double lt, double lg, double ltDistance, double lgDistance)
            {
                double ltCoef = ltDistance * 0.0000089;
                double lgCoef = lgDistance * 0.0000089;
                this.lt = lt + ltCoef;
                this.lg = lg + lgCoef / Math.Cos(lt * 0.018);
            }
        }

        private List<Loc> CreateSquarePolygon(double latitude, double longitude, double distance)
        {
            List<Loc> polygon = new List<Loc>()
            {
                new Loc(latitude, longitude, distance, distance),
                new Loc(latitude, longitude, distance, -distance),
                new Loc(latitude, longitude, -distance, -distance),
                new Loc(latitude, longitude, -distance, distance)
            };

            return polygon;
        }

        public async Task<List<PointDetail>> GetAllByDistanceAsync(string serverUrl, long userId, double latitude, double longitude, double distance, bool details = false)
        {
            List<PointDetail> points = new List<PointDetail>();
            var distanceMeters = distance * 1000;
            var polygon = CreateSquarePolygon(latitude, longitude, distanceMeters);
            
            var maxLt = Convert.ToDecimal(polygon.Max(x => x.Lt));
            var minLt = Convert.ToDecimal(polygon.Min(x => x.Lt));
            var maxLg = Convert.ToDecimal(polygon.Max(x => x.Lg));
            var minLg = Convert.ToDecimal(polygon.Min(x => x.Lg));

            IEnumerable<Data.Models.InfoPost> infoPosts;

            if (!details)
            {
                infoPosts = await _infoPostRepository.GetManyAsync(x => x.Latitude <= maxLt && minLt <= x.Latitude && x.Longitude <= maxLg && minLg <= x.Longitude, false);
            }
            else
            {
                infoPosts = await _infoPostRepository.GetAllAsync(x => x.Latitude <= maxLt && minLt <= x.Latitude && x.Longitude <= maxLg && minLg <= x.Longitude, c => c.VotePost, c => c.BelongToNavigation);
            }

            foreach (var info in infoPosts)
            {
                var isIn1 = IsPointInPolygon(polygon, new Loc(info.Latitude, info.Longitude));

                //var isIn2 = IsPointInPolygon2(polygon, new Loc(info.Latitude, info.Longitude));
                
                if (isIn1)
                {
                    if (!details)
                    {
                        points.Add(await GetShortPointDetail(serverUrl, info, userId));
                    }
                    else
                    {
                        points.Add(await GetPointDetail(serverUrl, info, userId));
                    }
                }
            }

            return points;
        }

        private async Task<PointDetail> GetShortPointDetail(string serverUrl, Data.Models.InfoPost info, long userId)
        {
            return new PointDetail()
            {
                Id = info.Id,
                Latitude = Convert.ToDouble(info.Latitude),
                Longitude = Convert.ToDouble(info.Longitude),
                ImageUrls = await GetImagesAsync(serverUrl, info.Id, "48"),
                Description = info.Description,
                CreationTime = info.CreationTime,
                FixedTime = info.FixedTime,
                IsApproved = info.IsApproved.HasValue ? info.IsApproved.Value : false
            };
        }

        private async Task<PointDetail> GetPointDetail(string serverUrl, Data.Models.InfoPost info, long userId)
        {
            return new PointDetail()
            {
                Id = info.Id,
                Latitude = Convert.ToDouble(info.Latitude),
                Longitude = Convert.ToDouble(info.Longitude),
                ImageUrls = await GetImagesAsync(serverUrl, info.Id, "256"),
                Description = info.Description,
                ApprovedTime = info.ApprovedTime,
                BelongToName = info.BelongToNavigation?.Name ?? null,
                CreationTime = info.CreationTime,
                FixedByName = info.FixedByNavigation?.Name ?? null,
                FixedTime = info.FixedTime,
                IsApproved = info.IsApproved.HasValue ? info.IsApproved.Value : false,
                DeviceId = info.User?.DeviceId ?? null,
                Vote = info.VotePost?.FirstOrDefault(x => x.UserId == userId)?.Vote ?? null,
                Likes = info.VotePost?.Count(x => x.Vote) ?? 0,
                Dislikes = info.VotePost?.Count(x => !x.Vote) ?? 0
            };
        }

        private PointVotes GetPointVotes(IEnumerable<VotePost> votes, long userId)
        {
            return new PointVotes()
            {
                PostId = votes.FirstOrDefault().InfoPostId,
                Vote = votes.FirstOrDefault(x => x.UserId == userId)?.Vote ?? null,
                Likes = votes.Count(x => x.Vote),
                Dislikes = votes.Count(x => !x.Vote)
            };
        }

        public async Task<List<PointDetail>> GetAllAsync(string serverUrl, long userId, bool details = false)
        {
            IEnumerable<Data.Models.InfoPost> infoPosts;

            if (!details)
            {
                infoPosts = await _infoPostRepository.GetAllAsync(false);
            }
            else
            {
                infoPosts = await _infoPostRepository.GetAllAsync(x => 1 == 1, c => c.VotePost, c => c.BelongToNavigation);
            }

            List<PointDetail> points = new List<PointDetail>();

            foreach (var info in infoPosts)
            {
                if (!details)
                {
                    points.Add(await GetShortPointDetail(serverUrl, info, userId));
                }
                else
                {
                    points.Add(await GetPointDetail(serverUrl, info, userId));
                }
            }

            return points;
        }

        private async Task<List<string>> GetImagesAsync(string serverUrl, long infoId, string imgType)
        {
            var images = await _imageRepository.GetImagesByInfoIdAsync(infoId);

            return images.Select(y => serverUrl + "/" + y.Path + "/" + imgType + ".png").ToList();
        }

        public async Task<bool> IsApprovedAsync(long id, bool isApproved)
        {
            var infoPost = await _infoPostRepository.ExistsAsync(id);

            if (!infoPost)
            {
                return false;
            }

            var success = await _infoPostRepository.IsApprovedAsync(id, isApproved);

            return success;
        }

        public async Task VoteAsync(long userId, long postId, bool vote, bool undo)
        {
            //var infoPost = await _infoPostRepository.ExistsAsync(postId);

            //if (!infoPost)
            //{
            //    throw new Exception($"Post with id {postId} not found!!!");
            //}

            //var user = await _registerService.GetUserByIdAsync(userId);

            //if (user == null)
            //{
            //    throw new Exception($"User with id {userId} not found!!!");
            //}

            try
            {
                await _votePostRepository.RemoveByPostIdUserIdAsync(postId, userId);

                if (!undo)
                {
                    await _votePostRepository.AddAsync(new VotePost()
                    {
                        UserId = userId,
                        InfoPostId = postId,
                        Vote = vote
                    });
                }
            }
            catch (Exception e)
            {

            }
        }

        //public DateTime GetDateTaken(Image targetImg)
        //{
        //    DateTime dtaken;

        //    try
        //    {
        //        //Property Item 306 corresponds to the Date Taken
        //        PropertyItem propItem = targetImg.GetPropertyItem(0x0132);

        //        //Convert date taken metadata to a DateTime object
        //        string sdate = Encoding.UTF8.GetString(propItem.Value).Trim();
        //        string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
        //        string firsthalf = sdate.Substring(0, 10);
        //        firsthalf = firsthalf.Replace(":", "-");
        //        sdate = firsthalf + secondhalf;
        //        dtaken = DateTime.Parse(sdate);
        //    }
        //    catch
        //    {
        //        dtaken = DateTime.Parse("1956-01-01 00:00:00.000");
        //    }
        //    return dtaken;
        //}
    }
}
