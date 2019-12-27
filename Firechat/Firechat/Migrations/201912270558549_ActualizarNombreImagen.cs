namespace Firechat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarNombreImagen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ImagenUrl", c => c.String());
            DropColumn("dbo.AspNetUsers", "Imagen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Imagen", c => c.String());
            DropColumn("dbo.AspNetUsers", "ImagenUrl");
        }
    }
}
