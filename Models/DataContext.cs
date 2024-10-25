namespace Asp.Net_MvcWeb_Pj3.Aptech.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// Lớp ngữ cảnh dữ liệu, mô hình hóa MySQL DB Server
// KHông nói gì thì nó để mã hóa là: latin1_swedish_ci
// mà cái mình cần thì là: utf8mb4
// using MySql.EntityFrameworkCore.Extensions;

public class DataContext : DbContext
{
    // Liệt kê, mô hình hóa các bảng trong DB
    // Liệt kê các tập thực thể, 
    // tương ứng với các bảng trong cơ sở dữ liệu

    
    public DbSet<Publisher> Publisher {get;set;}
    public DbSet<Patient> Patient {get;set;}


     public DataContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Kết nối cơ sở dữ liệu SQLite3
        // options.UseSqlite("Data Source=.\\db.sqlite3"); // ít nhất cũng khởi tạo dữ liệu ban đầu được.
        options.UseSqlite(@"Data Source=.\data.db"); // ít nhất cũng khởi tạo dữ liệu ban đầu được.
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // https://learn.microsoft.com/en-us/answers/questions/613149/how-to-add-tables-from-models-to-applicationdbcont
        modelBuilder.Entity<Publisher>().ToTable("Publisher"); 
        modelBuilder.Entity<Patient>().ToTable("Patient");  
         
        // modelBuilder.HasCharSet("utf8mb4");

        // modelBuilder.Entity<User>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        // });

        // Chỉ ra ràng buộc dữ liệu cho bảng con Sach
        // Câu lệnh C# để tạo ràng buộc Nhiều-Một giữa 
        // 2 thực thể được đặt trong lớp ngữ cảnh, chứ
        // không phải lớp thực thể
        // modelBuilder.Entity<Sach>(entity =>
        // {
        //     entity.HasKey(e => e.ID);
        //     entity.HasOne(d => d.NXB);
          
        // });
    }
    
}
