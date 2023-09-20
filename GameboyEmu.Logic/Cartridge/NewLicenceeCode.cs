﻿using System.Text;

namespace GameboyEmu.Logic.Cartridge;

public class NewLicenceeCode
{
    private readonly bool _useNewLicenceeCode;
    private readonly byte[] _value;

    public NewLicenceeCode(byte[] value, bool useNewLicenceeCode)
    {
        _useNewLicenceeCode = useNewLicenceeCode;
        _value = value;
    }
    
    public string ByteString() => string.Join(",", _value.Select(v =>v.ToString("X2")));

    private string Code => Encoding.ASCII.GetString(_value);

    public override string ToString()
    {
        if (!_useNewLicenceeCode)
        {
            return "N/A";
        }

        return Code switch
        {
            "00" => "None",
            "01" => "Nintendo R&D1",
            "08" => "Capcom",
            "13" => "Electronic Arts",
            "18" => "Hudson Soft",
            "19" => "b-ai",
            "20" => "kss",
            "22" => "pow",
            "24" => "PCM Complete",
            "25" => "san-x",
            "28" => "Kemco Japan",
            "29" => "seta",
            "30" => "Viacom",
            "31" => "Nintendo",
            "32" => "Bandai",
            "33" => "Ocean/Acclaim",
            "34" => "Konami",
            "35" => "Hector",
            "37" => "Taito",
            "38" => "Hudson",
            "39" => "Banpresto",
            "41" => "Ubi Soft",
            "42" => "Atlus",
            "44" => "Malibu",
            "46" => "angel",
            "47" => "Bullet-Proof",
            "49" => "irem",
            "50" => "Absolute",
            "51" => "Acclaim",
            "52" => "Activision",
            "53" => "American sammy",
            "54" => "Konami",
            "55" => "Hi tech entertainment",
            "56" => "LJN",
            "57" => "Matchbox",
            "58" => "Mattel",
            "59" => "Milton Bradley",
            "60" => "Titus",
            "61" => "Virgin",
            "64" => "LucasArts",
            "67" => "Ocean",
            "69" => "Electronic Arts",
            "70" => "Infogrames",
            "71" => "Interplay",
            "72" => "Broderbund",
            "73" => "sculptured",
            "75" => "sci",
            "78" => "THQ",
            "79" => "Accolade",
            "80" => "misawa",
            "83" => "lozc",
            "86" => "Tokuma Shoten Intermedia",
            "87" => "Tsukuda Original",
            "91" => "Chunsoft",
            "92" => "Video system",
            "93" => "Ocean/Acclaim",
            "95" => "Varie",
            "96" => "Yonezawa/s’pal",
            "97" => "Kaneko",
            "99" => "Pack in soft",
            "A4" => "Konami (Yu-Gi-Oh!)",
            _ => Code
        };
    }
}