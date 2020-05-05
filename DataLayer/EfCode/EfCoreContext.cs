using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EfCode
{
    public class EfCoreContext : DbContext
    {
        public DbSet<AreaOfInterest> AreaOfInterests { get; set; }
        public DbSet<AreaOfInterestReviewer> AreaOfInterestReviewers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorManuscript> AuthorManuscripts { get; set; }
        public DbSet<Editor> Editors { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Manuscript> Manuscripts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        public EfCoreContext(DbContextOptions<EfCoreContext> options) : base(options)
        {
        }
        public EfCoreContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                    connectionString:
                    @"Data Source=MAJDINURHASAN-L\SQLEXPRESS;Integrated Security=True;Database=JournalDB;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaOfInterest>(c =>
            {
                c.ToTable("AreaOfInterest");
                c.HasKey(d => d.AreaOfInterestId);

                c.Property(d => d.AreaOfInterestId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AreaOfInterestReviewer>(c =>
            {
                c.ToTable("AreaOfInterestReviewer");
                c.HasKey(d => d.AreaOfInterestReviewerId);

                c.Property(d => d.AreaOfInterestReviewerId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                c.Property(d => d.ReviewerId)
                    .IsRequired();

                c.HasOne(d => d.AreaOfInterest)
                    .WithMany(d => d.AreaOfInterestReviewers)
                    .HasForeignKey(d => d.AreaOfInterestId);

                c.HasOne(d => d.Reviewer)
                    .WithMany(d => d.AreaOfInterestReviewers)
                    .HasForeignKey(d => d.ReviewerId);
            });

            modelBuilder.Entity<Author>(c =>
            {
                c.ToTable("Author");
                c.HasKey(d => d.AuthorId);

                c.Property(d => d.AuthorId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AuthorManuscript>(c =>
            {
                c.ToTable("AuthorManuscript");
                c.HasKey(d => d.AuthorManuscriptId);

                c.Property(d => d.AuthorManuscriptId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                c.Property(d => d.AuthorId)
                    .IsRequired();

                c.Property(d => d.ManuscriptId)
                    .IsRequired();

                c.HasOne(d => d.Author)
                    .WithMany(d => d.AuthorManuscripts)
                    .HasForeignKey(d => d.AuthorId);

                c.HasOne(d => d.Manuscript)
                    .WithMany(d => d.AuthorManuscripts)
                    .HasForeignKey(d => d.ManuscriptId);
            });

            modelBuilder.Entity<Editor>(c =>
            {
                c.ToTable("Editor");
                c.HasKey(d => d.EditorId);

                c.Property(d => d.EditorId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Issue>(c =>
            {
                c.ToTable("Issue");
                c.HasKey(d => d.IssueId);

                c.Property(d => d.IssueId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Manuscript>(c =>
            {
                c.ToTable("Manuscript");
                c.HasKey(d => d.ManuscriptId);

                c.Property(d => d.ManuscriptId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                c.HasOne(d => d.Issue)
                    .WithMany(d => d.Manuscripts)
                    .HasForeignKey(d => d.IssueId);

                c.HasOne(d => d.Editor)
                    .WithMany(d => d.Manuscripts)
                    .HasForeignKey(d => d.EditorId);
            });

            modelBuilder.Entity<Notification>(c =>
            {
                c.ToTable("Notification");
                c.HasKey(d => d.NotificationId);

                c.Property(d => d.NotificationId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                c.HasOne(d => d.Author)
                    .WithMany(d => d.Notifications)
                    .HasForeignKey(d => d.AuthorId);
            });

            modelBuilder.Entity<Review>(c =>
            {
                c.ToTable("Review");
                c.HasKey(d => d.ReviewId);

                c.Property(d => d.ReviewId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                c.HasOne(d => d.Manuscript)
                    .WithMany(d => d.Reviews)
                    .HasForeignKey(d => d.ManuscriptId);

                c.HasOne(d => d.Reviewer)
                    .WithMany(d => d.Reviews)
                    .HasForeignKey(d => d.ReviewerId);
            });

            modelBuilder.Entity<Reviewer>(c =>
            {
                c.ToTable("Reviewer");
                c.HasKey(d => d.ReviewerId);

                c.Property(d => d.ReviewerId)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
