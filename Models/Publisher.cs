namespace Asp.Net_MvcWeb_Pj3.Aptech.Models;

using System.ComponentModel.DataAnnotations; // Key
using System.ComponentModel.DataAnnotations.Schema; // Foreign Key


// mã thực thể: Nhân Viên Quản Trị
// 2024.10.02 15h40
public
class Publisher
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

}

