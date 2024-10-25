namespace Asp.Net_MvcWeb_Pj3.Aptech.Models;

using System.ComponentModel.DataAnnotations; // Key
using System.ComponentModel.DataAnnotations.Schema; // Foreign Key


// mã thực thể: Nhân Viên Quản Trị
// 2024.10.02 15h40
public
class Patient
{
    [Key]
    public int Id { get; set; }
    // mã định danh

    public string Name { get; set; }
    // tựa đề
    
    public int Year { get; set; }
    // năm xuất bản

    public float Height {get;set;}
    // image url, image link, địa chỉ online của ảnh

    public char Boold_Type {get;set;}

    public int GenDer {get;set;}

    public bool Status {get;set;}

    public DateTime ImportDate {get;set;}
    // ngày nhập kho

    public DateTime? ExportDate  {get;set;} 
    // public DateTime CreatedAt { get; set; }
    // Ngày tạo
    // public DateTime UpdatedAt { get; set; }
    // Ngày sửa

    public int PublisherId { get; set; }
    // // Xem DataContext.cs để hiểu khóa ngoại sinh ra như nào
    // // DotNet tự động sinh ra cột có tên: KhoaID là khóa ngoại trỏ sang bảng NhaXuatBan(ID)
    // // Thuộc Tính này chứa dữ liệu của bảng ngoại, nhưng dotnet không đọc
    // // tự động, mà mình phải viết tường minh bằng lệnh Inlucde(bn => bn.Khoa)
    [ForeignKey("PublisherId")]
    public virtual Publisher Pub { get; set; }

    // Trường thông tin phái sinh

    // Hàm khởi tạo
    public Patient()
    {
        // this.Status = true;
        // this.Name = "Unknown";
        // this.Year = 2000;
        // this.Height = 0.1f;
        // this.Status = true; // new, false=old
        // this.CreatedAt = DateTime.Now;
        // this.UpdatedAt = DateTime.Now;

        // this.DateCreated = DateTime.ParseExact("23/12/2004", "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

        // Trường thông tin phái sinh
    public String StatusText
    {
        get
        {
            return this.Status == true ? "hospitalized" : "discharged";
        }
    }

       public string GenDerText
    {
        get
        {
            return this.GenDer == 1 ? "male" : "female";
        }
    }

    /**
     * Trả về định dạng yyyy-MM-dd cho thẻ <input type="date" value="@Model.ImportDateSQL" />
     */
    public String ImportDateSQL
    {
        get
        {
            return this.ImportDate.ToString("yyyy-MM-dd"); ;
        }
    }

    /**
     * Trả về định dạng vietnamese, để hiển thị trên Table View
     */
    public String ImportDateVi
    {
        get
        {
            return this.ImportDate.ToString("dd/MM/yyyy"); ;
        }
    }

   public string ExportDateSQL
    {
        get
        {
            return this.ExportDate.HasValue ? this.ExportDate.Value.ToString("yyyy-MM-dd") : string.Empty;
        }
    }

    public string ExportDateVi
    {
        get
        {
            return this.ExportDate.HasValue ? this.ExportDate.Value.ToString("dd/MM/yyyy") : string.Empty;
        }
    }
    

    public bool Invalid()
    {
        bool invalid = false;
        if (this.Name.Length < 2)
        {
            invalid = true;
            Write("Lỗi->Tên không được dưới 2 kí tự");

        }

        if (this.Name.Length > 32)
        {
            invalid = true;
            Write("Lỗi->Tên không được quá 32 kí tự");

        }

        return invalid;
    }
}

