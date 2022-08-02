using Core.Entities.Concrete;
using Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Massages
    {
        public static string ProductAdded = "Urun Eklendi";
        public static string ProductNameValid = "Urun ismi gecersiz";
        internal static string MaintenanceTime = "Sistem Bakimda.";
        internal static string ProductsListed = "Urunler Listelendi.";
        public static string ProductCategoryError = "Bir Kategoride en fazla 10 urun olabilir.";
        public static string ProductNameAlreadyExists = "Bu isimde zaten baska bir urun var.";
        public static string CategoryLimitExceded = "Kategori Asildigi icin yeni urun eklenemiyor.";
        public static string AuthorizationDenied = "Yetkiniz Yok.";
        public static string UserRegistered = "KAyit Olundu.";
        public static string UserNotFound = "Kullanici Bulunamadi.";
        public static string PasswordError = "Parola Hatasi.";
        public static string SuccessfulLogin = "Basarili Giris.";
        public static string UserAlreadyExists = "Kullanici Mevcut";
        public static string AccessTokenCreated = "Token Craated";
    }
}
