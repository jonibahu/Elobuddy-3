using Ass_Fiora.Model.Enum;

namespace Ass_Fiora.Model.Languages
{
    class Turkish : LanguageController
    {
        public Turkish()
        {
            Dictionary.Add(EnumContext.Version, "Sürüm: ");
            Dictionary.Add(EnumContext.Creator, "Yapımcı: Vector");
            Dictionary.Add(EnumContext.Draw, "Çizgiler");
            Dictionary.Add(EnumContext.TurnOffDraws, "Bütün çizgileri devre dışı brak");
            Dictionary.Add(EnumContext.Range, "Menzil");//Spell range
            Dictionary.Add(EnumContext.Combo, "Kombo");
            Dictionary.Add(EnumContext.Use, "Kullan");// Use spell
            Dictionary.Add(EnumContext.Harass, "Dürt(Harras)");
            Dictionary.Add(EnumContext.MinimunMana, "Aktif etmek için minimum % Mana");
            Dictionary.Add(EnumContext.Settings, "Ayarlar");
            Dictionary.Add(EnumContext.OnlyInPassiverange, "Sadece pasif menzilde");
            Dictionary.Add(EnumContext.LastHit, "Son vuruş");
            Dictionary.Add(EnumContext.LaneClear, "Lane temizleme");
            Dictionary.Add(EnumContext.NotificationMessage, " Yüklendi.");
        }
    }
}
