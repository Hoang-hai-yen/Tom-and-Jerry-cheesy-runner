# 🚀 Jerry's Infinite Run: Endless Runner Project

Dự án này là một game thể loại **Endless Runner (Chạy Vô Tận)** 3D được phát triển trên Unity. Người chơi điều khiển Jerry trong một cuộc rượt đuổi không hồi kết, né tránh chướng ngại vật và thu thập phô mai.

Trò chơi được xây dựng tập trung vào hiệu suất bằng cách sử dụng Object Pooling và tạo môi trường vô tận theo thủ tục.

## ✨ Tính Năng Cốt Lõi

* **Chạy Vô tận theo Thủ tục (Procedural Generation):** Tự động sinh và ghép nối các đoạn đường (Map Chunks) liên tục được tái sử dụng từ pool.
* **Điều khiển Cơ bản:** Cho phép người chơi di chuyển giữa ba làn đường, nhảy, và trượt để vượt qua chướng ngại vật.
* **Power-up Đa dạng:** Cung cấp các vật phẩm tăng cường như Nam châm (hút phô mai), Tăng tốc (vượt chướng ngại vật), Khiên (bảo vệ một lần), và Nhân điểm tạm thời.
* **Cơ chế Game Over Đuổi bắt:** Sau khi va chạm vật cản, người chơi dừng lại, kích hoạt kẻ thù (Tom) đuổi bắt có hẹn giờ, dẫn đến Game Over nếu bị bắt kịp.
* **Tăng tốc độ theo thời gian:** Tốc độ chạy mặc định của nhân vật sẽ tăng dần theo thời gian chơi.
* **Hệ thống Điểm số:** Theo dõi và hiển thị điểm số (phô mai) đã thu thập, bao gồm cả nhân điểm.

## 🎮 Phím Điều Khiển (PC)

Người chơi sử dụng các phím mũi tên để điều khiển nhân vật Jerry:

| Thao tác | Phím | Logic trong Script |
| :--- | :--- | :--- |
| **Nhảy (Jump)** | Mũi tên **Lên** (`UpArrow`) | Kích hoạt khi nhân vật đang chạm đất (`isGrounded`). |
| **Trượt (Slide)** | Mũi tên **Xuống** (`DownArrow`) | Thay đổi kích thước collider để trượt qua vật cản thấp. |
| **Chuyển làn Trái** | Mũi tên **Trái** (`LeftArrow`) | Di chuyển sang làn đường bên trái. |
| **Chuyển làn Phải** | Mũi tên **Phải** (`RightArrow`) | Di chuyển sang làn đường bên phải. |

## 🛠️ Công Nghệ và Kỹ Thuật

| Hạng mục | Công nghệ/Kỹ thuật | Chi tiết |
| :--- | :--- | :--- |
| **Engine** | Unity Engine (C#) | Nền tảng phát triển chính. |
| **Procedural Generation** | Object Pooling + Random Weighted Pattern | Tối ưu hóa hiệu suất và tạo sự đa dạng cho đường chạy. |
| **Đồ họa** | Pixel Cartoon Style tự vẽ | Phong cách đồ họa được sử dụng. |
| **Âm thanh** | Freesound | Nguồn tài nguyên âm thanh. |
| **Quản lý Mã nguồn** | Github | Quản lý phiên bản và theo dõi thay đổi. |

## 📦 Cấu trúc Script Cốt lõi

| File | Chức năng |
| :--- | :--- |
| `PlayerMovement.cs` | Logic điều khiển người chơi (di chuyển, nhảy, trượt, quản lý buff). |
| `TomFollower.cs` | Logic AI kẻ thù, xử lý cơ chế đuổi bắt Game Over. |
| `MapSpawner.cs` | Quản lý việc sinh và hủy các Map Chunk vô tận. |
| `ItemPoolManager.cs` | Hệ thống Object Pooling trung tâm cho các vật phẩm và Pattern. |
| `ScoreManager.cs` | Theo dõi và quản lý điểm số, bao gồm cả nhân điểm. |
| `GameOverManager.cs` | Xử lý việc dừng game và hiển thị điểm cuối cùng. |

## 💻 Thiết lập và Khởi động Project

### Yêu cầu

* Unity Editor (Phiên bản tương thích).
* Visual Studio hoặc IDE tương thích với C#.

### Hướng dẫn

1.  **Clone Repository:**
    ```bash
    git clone [LINK_REPOSITORY_CỦA_BẠN]
    ```
2.  **Mở Project:** Mở thư mục dự án bằng Unity Hub.
3.  **Kiểm tra Thư viện:** Đảm bảo thư viện TextMeshPro đã được Import vào dự án.
4.  **Chạy Scene Chính:** Mở Scene chính (Scene có chứa `MapSpawner`, `Player`, `ItemPoolManager`,...) và nhấn Play trong Unity Editor.
