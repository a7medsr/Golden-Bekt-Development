using Microsoft.EntityFrameworkCore;

namespace Golden_Bekt_Development.Models.Context
{
    public class GoldenDbContext:DbContext
    {
        public GoldenDbContext(DbContextOptions<GoldenDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Annual_report> Annual_reports { get; set; }
        public virtual DbSet<Association_Member> Association_Members { get; set; }
        public virtual DbSet<Association_Minute> Association_Minutes { get; set; }
        public virtual DbSet<Board_of_Director> Board_Of_Directors { get; set; }
        public virtual DbSet<Branche> Branches { get; set; }
        public virtual DbSet<Commercial_register> Commercial_Registers { get; set; }
        public virtual DbSet<Committee> Committees { get; set; }
        public virtual DbSet<Disclosure> Disclosures { get; set; }
        public virtual DbSet<Financial_report> Financial_Reports { get; set; }
        public virtual DbSet<Investment> Investments { get; set; }
        public virtual DbSet<Library> Libraries { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Our_news> Our_News { get; set; }
        public virtual DbSet<Partners_of_Success> Partners_Of_Successes { get; set; }
        public virtual DbSet<Policies_and_regulations> Policies_And_Regulations { get; set; }
        public virtual DbSet<programs> Programs { get; set; }
        public virtual DbSet<said_about_us> Said_About_Us { get; set; }
        public virtual DbSet<Satisfaction_measurement> Satisfaction_Measurements { get; set; }
        public virtual DbSet<service> Services { get; set; }
        public virtual DbSet<Statistics> Statistics { get; set; }
        public virtual DbSet<Strategic_and_operational_objectives> Strategic_And_Operational_Objectives { get; set; }
        public virtual DbSet<Vacancies> Vacancies { get; set; }
        //public virtual DbSet<Common> Commons { get; set; }  





    }
}
