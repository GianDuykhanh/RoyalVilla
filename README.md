# RoyalVilla

## Giới thiệu

RoyalVilla là ứng dụng quản lý biệt thự / villa (khách sạn nhỏ) được xây dựng trên **.NET 10** (ASP.NET Core + Web API + Frontend MVC).

- **API Backend**: `RoyalVillaAPI` (Web API + OpenAPI/Scalar + JWT Authentication)
- **Frontend**: `RoyalVillaWeb` (MVC Razor Pages + jQuery + Bootstrap)
- **Shared DTO**: `RoyalVillaDTO`

## Công nghệ sử dụng

- **.NET 10.0** (Target Framework)
- **Entity Framework Core 10** + **SQL Server**
- **JWT Authentication** (Bearer)
- **AutoMapper 16**
- **Scalar API Reference** (thay cho Swagger)
- **CORS** (cho phép tất cả)
- **Seed data** tự động khi chạy lần đầu

## Cấu trúc dự án

```
RoyalVilla/
├── RoyalVilla.sln               # Solution
├── RoyalVillaAPI/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── RoyalVillaAPI.csproj
│   ├── Data/ApplicationDbContext.cs
│   ├── Controllers/
│   │   ├── VillaController.cs
│   │   ├── VillaAmenitiesController.cs
│   │   └── AuthController.cs
│   ├── Models/ (Villa, User, VillaAmenities)
│   ├── Services/ (AuthService, IAuthService)
│   └── Migrations/
├── RoyalVillaWeb/
│   ├── Program.cs
│   ├── RoyalVillaWeb.csproj
│   ├── Services/ (VillaService, BaseService...)
│   ├── Controllers/HomeController.cs
│   ├── Views/ (MVC Razor)
│   └── wwwroot/ (CSS, JS, lib)
├── RoyalVillaDTO/ (Shared DTOs: VillaDTO, LoginRequestDTO...)
└── README.md
```

## Cài đặt & Chạy dự án

### 1. Chuẩn bị

- **SQL Server** (phiên bản 2016+)
- **Database**: `RoyalVilla`
- **User**: `sa` hoặc account có quyền (đã dùng `Trusted_Connection=True`)

### 2. Cấu hình

1. **Tạo database**:
   ```sql
   CREATE DATABASE RoyalVilla;
   ```

2. **Cấu hình ConnectionString** (nếu không dùng User Secrets):
   - Chỉnh file `RoyalVillaAPI/appsettings.json` hoặc `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=DESKTOP-4EV3C97\\SERVER17;Database=RoyalVilla;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

### 3. Build & Run

**Chạy toàn bộ solution**:
```bash
dotnet run --project RoyalVillaAPI
dotnet run --project RoyalVillaWeb
```

**Chạy riêng API** (để frontend gọi):
```bash
cd RoyalVillaAPI
dotnet run --urls "http://localhost:5000"
```

**Chạy riêng Web**:
```bash
cd RoyalVillaWeb
dotnet run --urls "http://localhost:5001"
```

Sau khi chạy:
- **API**: `http://localhost:5000` (có Scalar API Reference tại `/swagger`)
- **Web Frontend**: `http://localhost:5001`

## Đăng nhập & Sử dụng

### Auth (JWT)
- **Register**: `POST /api/auth/register`
- **Login**: `POST /api/auth/login` → trả về JWT token
- **Header**: `Authorization: Bearer <token>`

### Villa API
- `GET /api/villa` (lấy danh sách villa)
- `POST /api/villa` (tạo villa)
- `PUT /api/villa/{id}` / `PATCH /api/villa/{id}`
- `DELETE /api/villa/{id}`

**Dùng Postman / Insomnia** hoặc Swagger (Scalar):
- Truy cập Scalar: `http://localhost:5000/swagger`

## Seed Data
Dữ liệu mẫu (Villa + User) được seed tự động khi chạy lần đầu.

## Mô hình dữ liệu

- **Villa** (Id, Name, Price, Details, ImageUrl, Occupancy, CreatedDate...)
- **VillaAmenities** (Id, VillaId, Name, Details)
- **User** (Id, Name, Email, Phone, PasswordHash...)

---

**Cập nhật lần cuối**: 18/08/2026