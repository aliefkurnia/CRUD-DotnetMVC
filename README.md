# **Order Management System**  

## **📌 Overview**  
Order Management System adalah aplikasi berbasis web yang dikembangkan menggunakan **.NET Core C#** dan **SQL Server**. Aplikasi ini memungkinkan pengguna untuk **mengelola pesanan dan item**, termasuk fitur pencarian, ekspor data, serta CRUD (Create, Read, Update, Delete).  

## **📸 Fitur Utama**  
✅ **Order List**  
- 🔍 Pencarian berdasarkan **keyword** dan **order date**  
- 📅 Pilihan tanggal menggunakan **Date Picker**  
- ➕ **Tambah Data**: Redirect ke halaman pembuatan order  
- 📥 **Export**: Mengunduh data dalam **format Excel**  
- ✏️ **Edit Order**: Mengedit order dan item yang sudah ada  
- 🗑️ **Hapus Order**: Konfirmasi sebelum menghapus order dan item  

✅ **Item Management**  
- 🔢 **Nomor Order**: Input teks bebas  
- 📅 **Tanggal Order**: Date Picker  
- ➕ **Tambah Item**: Menambah item secara **inline di grid**  
- 💾 **Simpan Item**: Menyimpan data item sementara  
- ❌ **Batal**: Menutup grid input/edit  
- ✏️ **Edit Item**: Mengubah data item yang sudah ada  
- 🗑️ **Hapus Item**: Menghapus item sementara  
- ✅ **Simpan Order & Item** ke database  
- 🔙 **Close**: Kembali ke daftar order  

## **🛠️ Teknologi yang Digunakan**  
- **.NET Core / .NET Framework (C#)**  
- **SQL Server**  
- **Entity Framework Core**  
- **Bootstrap / UI Template (Bebas memilih)**
- **MVC (Model-View-Controller)**  

## **📂 Struktur Folder**  
```
/CRUD-EF
│── /Controllers
│── /Models
│── /Views
│── /Services
│── /wwwroot
│── appsettings.json
│── Program.cs
│── README.md
```

## **🚀 Cara Menjalankan Aplikasi**  
### **1️⃣ Clone Repository**  
```sh
git clone https://github.com/aliefkurnia/CRUD-DotnetMVC
```

### **2️⃣ Konfigurasi Database**  
- **Buat database di SQL Server** sesuai dengan `schema.sql`  
- **Update `appsettings.json`** dengan connection string database  

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=OrderDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}
```

### **3️⃣ Jalankan Migration dan Seed Data**  
```sh
dotnet ef database update
```

### **4️⃣ Jalankan Aplikasi**  
```sh
dotnet run
```
Akses di browser: **`http://localhost:5000`**  

## **📦 Deployment**  
- **Build aplikasi:**  
  ```sh
  dotnet publish -c Release -o publish
  ```

## **📜 Database Schema**  
Gunakan file `schema.sql` yang sudah diberikan dalam requirement.  
