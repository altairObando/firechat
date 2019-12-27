namespace Firechat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaActualizacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Imagen = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ConversacionEliminada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversacionId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Conversacion", t => t.ConversacionId, cascadeDelete: true)
                .Index(t => t.ConversacionId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Mensaje",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversacionId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        Contenido = c.String(),
                        FechaEnvio = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Conversacion", t => t.ConversacionId, cascadeDelete: true)
                .Index(t => t.ConversacionId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.MensajeEliminado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MensajeId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        FechaEliminacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Mensaje", t => t.MensajeId, cascadeDelete: true)
                .Index(t => t.MensajeId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Participacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        ConversacionId = c.Int(nullable: false),
                        TipoParticipacion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Conversacion", t => t.ConversacionId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ConversacionId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Participacion", "ConversacionId", "dbo.Conversacion");
            DropForeignKey("dbo.Participacion", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MensajeEliminado", "MensajeId", "dbo.Mensaje");
            DropForeignKey("dbo.MensajeEliminado", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Mensaje", "ConversacionId", "dbo.Conversacion");
            DropForeignKey("dbo.Mensaje", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConversacionEliminada", "ConversacionId", "dbo.Conversacion");
            DropForeignKey("dbo.ConversacionEliminada", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversacion", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Participacion", new[] { "ConversacionId" });
            DropIndex("dbo.Participacion", new[] { "ApplicationUserId" });
            DropIndex("dbo.MensajeEliminado", new[] { "ApplicationUserId" });
            DropIndex("dbo.MensajeEliminado", new[] { "MensajeId" });
            DropIndex("dbo.Mensaje", new[] { "ApplicationUserId" });
            DropIndex("dbo.Mensaje", new[] { "ConversacionId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ConversacionEliminada", new[] { "ApplicationUserId" });
            DropIndex("dbo.ConversacionEliminada", new[] { "ConversacionId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Conversacion", new[] { "ApplicationUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Participacion");
            DropTable("dbo.MensajeEliminado");
            DropTable("dbo.Mensaje");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.ConversacionEliminada");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Conversacion");
        }
    }
}
