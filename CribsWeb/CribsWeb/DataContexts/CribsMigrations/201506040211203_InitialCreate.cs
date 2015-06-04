namespace Cribs.Web.DataContexts.CribsMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CribImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                        Cover = c.Boolean(nullable: false),
                        RentCrib_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentCribs", t => t.RentCrib_Id, cascadeDelete: true)
                .Index(t => t.RentCrib_Id);
            
           
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CribImages", "RentCrib_Id", "dbo.RentCribs");
            DropIndex("dbo.CribImages", new[] { "RentCrib_Id" });
            DropTable("dbo.CribImages");
        }
    }
}
