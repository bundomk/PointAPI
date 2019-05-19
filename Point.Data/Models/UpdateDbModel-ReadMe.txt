DATABASE MODEL UPDATE STEPS:

0) Try to set on the project you are doing migration with right-click and "Set as startup project"

1) Open Package Manager Console (Tools -> NuGet Package Manager -> Package Manager Console)

2) Execute the following command:

Scaffold-DbContext "Server=sv70.dbsqlserver.com,1288;Database=PointAdvisor;User Id=dbuser;Password=hpV1n#34" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Project Point.Data -Force

3) Add the following constructor in the created FlightProviderContext.cs (or copy it before executing the command):

public PointAdvisorContext(DbContextOptions<PointAdvisorContext> options)
            : base(options)
        { }

Replace (bug will be fixed in EF2) for ZonePoints:

entity.Property(e => e.Latitude).HasColumnType("decimal(9,6)");
entity.Property(e => e.Longitude).HasColumnType("decimal(9,6)");

4) Remove the OnConfiguring(...) method from PointAdvisorContext.cs