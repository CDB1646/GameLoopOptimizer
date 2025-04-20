GameLoop Optimizer
GameLoop performansını artırmak için geliştirilmiş bir C# aracı. Bu araç, sistem performansını optimize eder, gereksiz hizmetleri kapatır, telemetriyi devre dışı bırakır ve GameLoop için özel ayarlar yapar.
Özellikler

Optimize Et: Sistem performansını artırır ve GameLoop için özel ayarlar yapar.
Geri Dön: Yapılan değişiklikleri geri alır.
GitHub Entegrasyonu: Tüm .reg dosyaları GitHub’dan otomatik olarak indirilir.

Kurulum

Bu repoyu klonlayın:
git clone https://github.com/[kullanıcı_adınız]/GameLoopOptimizer.git


Visual Studio’da GameLoopOptimizer.sln dosyasını açın (eğer yoksa, Visual Studio ile bir Windows Forms projesi oluşturun ve GameLoopOptimizer.cs dosyasını ekleyin).

GameLoopOptimizer.cs dosyasında privacyRegUrl, systemRegUrl, servicesRegUrl, gameRegUrl ve networkRegUrl değişkenlerini kendi GitHub reponuzun URL’leri ile güncelleyin.

Projeyi derleyin ve çalıştırın (Ctrl + F5).


Kullanım

Optimize Et butonuna basarak tüm optimizasyonları uygulayın.
Geri Dön butonuna basarak değişiklikleri geri alın.

Notlar

Programı yönetici haklarıyla çalıştırın.
Kayıt defteri değişiklikleri öncesi yedek alınır (C:\Backup\).
İnternet bağlantısı gereklidir (GitHub’dan .reg dosyalarını indirmek için).

Lisans
MIT Lisansı altında dağıtılmaktadır.
