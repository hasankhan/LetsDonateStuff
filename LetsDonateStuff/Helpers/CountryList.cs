﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages.Html;
using System.Drawing;

namespace LetsDonateStuff.Helpers
{
    public class Country
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public PointF Coordinates { get; private set; }

        public Country(string code, string name, double lat, double lng)
        {
            this.Code = code;
            this.Name = name;
            this.Coordinates = new PointF((float)lat, (float)lng);
        }
    }

    public class CountryList
    {
        public const string AllCountries = "All Countries";
        public const string AllCountriesCode = "All";

        public static System.Web.Mvc.SelectList GetSelectList()
        {
            var items = new[] { new Country(AllCountriesCode, AllCountries, 0, 0) }.Union(countries);
            var list = new System.Web.Mvc.SelectList(items, "Code", "Name");
            return list;
        }

        public static IEnumerable<Country> All
        {
            get { return countries; }
        }

        public static Country GetCountry(string code)
        {
            code = (code ?? "").ToUpperInvariant();

            Country item = countries.Where(c => c.Code == code)
                                    .FirstOrDefault();
            return item;
        }

        public static bool IsValidCode(string code)
        {
            bool isValid = GetCountry(code) != null;
            return isValid;
        }

        static readonly Country[] countries = new[]{
            new Country("AF", "Afghanistan", 33, 65),
            new Country("AL", "Albania", 41, 20),
            new Country("DZ", "Algeria", 28, 3),
            new Country("AS", "American Samoa", -14.3333, -170),
            new Country("AD", "Andorra", 42.5, 1.5),
            new Country("AO", "Angola", -12.5, 18.5),
            new Country("AI", "Anguilla", 18.25, -63.1667),
            new Country("AQ", "Antarctica", -90, 0),
            new Country("AG", "Antigua and Barbuda", 17.05, -61.8),
            new Country("AR", "Argentina", -34, -64),
            new Country("AM", "Armenia", 40, 45),
            new Country("AW", "Aruba", 12.5, -69.9667),
            new Country("AU", "Australia", -27, 133),
            new Country("AT", "Austria", 47.3333, 13.3333),
            new Country("AZ", "Azerbaijan", 40.5, 47.5),
            new Country("BS", "Bahamas", 24.25, -76),
            new Country("BH", "Bahrain", 26, 50.55),
            new Country("BD", "Bangladesh", 24, 90),
            new Country("BB", "Barbados", 13.1667, -59.5333),
            new Country("BY", "Belarus", 53, 28),
            new Country("BE", "Belgium", 50.8333, 4),
            new Country("BZ", "Belize", 17.25, -88.75),
            new Country("BJ", "Benin", 9.5, 2.25),
            new Country("BM", "Bermuda", 32.3333, -64.75),
            new Country("BT", "Bhutan", 27.5, 90.5),
            new Country("BO", "Bolivia", -17, -65),
            new Country("BA", "Bosnia and Herzegovina", 44, 18),
            new Country("BW", "Botswana", -22, 24),
            new Country("BV", "Bouvet Island", -54.4333, 3.4),
            new Country("BR", "Brazil", -10, -55),
            new Country("IO", "British Indian Ocean Territory", -6, 71.5),
            new Country("BN", "Brunei Darussalam", 4.5, 114.6667),
            new Country("BG", "Bulgaria", 43, 25),
            new Country("BF", "Burkina Faso", 13, -2),
            new Country("BI", "Burundi", -3.5, 30),
            new Country("KH", "Cambodia", 13, 105),
            new Country("CM", "Cameroon", 6, 12),
            new Country("CA", "Canada", 60, -95),
            new Country("CV", "Cape Verde", 16, -24),
            new Country("KY", "Cayman Islands", 19.5, -80.5),
            new Country("CF", "Central African Republic", 7, 21),
            new Country("TD", "Chad", 15, 19),
            new Country("CL", "Chile", -30, -71),
            new Country("CN", "China", 35, 105),
            new Country("CX", "Christmas Island", -10.5, 105.6667),
            new Country("CC", "Cocos (Keeling) Islands", -12.5, 96.8333),
            new Country("CO", "Colombia", 4, -72),
            new Country("KM", "Comoros", -12.1667, 44.25),
            new Country("CG", "Congo", -1, 15),
            new Country("CD", "Congo, The Democratic Republic of The", 0, 25),
            new Country("CK", "Cook Islands", -21.2333, -159.7667),
            new Country("CR", "Costa Rica", 10, -84),
            new Country("CI", "Cote D'ivoire", 8, -5),
            new Country("HR", "Croatia", 45.1667, 15.5),
            new Country("CU", "Cuba", 21.5, -80),
            new Country("CY", "Cyprus", 35, 33),
            new Country("CZ", "Czech Republic", 49.75, 15.5),
            new Country("DK", "Denmark", 56, 10),
            new Country("DJ", "Djibouti", 11.5, 43),
            new Country("DM", "Dominica", 15.4167, -61.3333),
            new Country("DO", "Dominican Republic", 19, -70.6667),
            new Country("EC", "Ecuador", -2, -77.5),
            new Country("EG", "Egypt", 27, 30),
            new Country("SV", "El Salvador", 13.8333, -88.9167),
            new Country("GQ", "Equatorial Guinea", 2, 10),
            new Country("ER", "Eritrea", 15, 39),
            new Country("EE", "Estonia", 59, 26),
            new Country("ET", "Ethiopia", 8, 38),
            new Country("FK", "Falkland Islands (Malvinas)", -51.75, -59),
            new Country("FO", "Faroe Islands", 62, -7),
            new Country("FJ", "Fiji", -18, 175),
            new Country("FI", "Finland", 64, 26),
            new Country("FR", "France", 46, 2),
            new Country("GF", "French Guiana", 4, -53),
            new Country("PF", "French Polynesia", -15, -140),
            new Country("TF", "French Southern Territories", -43, 67),
            new Country("GA", "Gabon", -1, 11.75),
            new Country("GM", "Gambia", 13.4667, -16.5667),
            new Country("GE", "Georgia", 42, 43.5),
            new Country("DE", "Germany", 51, 9),
            new Country("GH", "Ghana", 8, -2),
            new Country("GI", "Gibraltar", 36.1833, -5.3667),
            new Country("GR", "Greece", 39, 22),
            new Country("GL", "Greenland", 72, -40),
            new Country("GD", "Grenada", 12.1167, -61.6667),
            new Country("GP", "Guadeloupe", 16.25, -61.5833),
            new Country("GU", "Guam", 13.4667, 144.7833),
            new Country("GT", "Guatemala", 15.5, -90.25),
            new Country("GN", "Guinea", 11, -10),
            new Country("GW", "Guinea-bissau", 12, -15),
            new Country("GY", "Guyana", 5, -59),
            new Country("HT", "Haiti", 19, -72.4167),
            new Country("HM", "Heard Island and Mcdonald Islands", -53.1, 72.5167),
            new Country("VA", "Holy See (Vatican City State)", 41.9, 12.45),
            new Country("HN", "Honduras", 15, -86.5),
            new Country("HK", "Hong Kong", 22.25, 114.1667),
            new Country("HU", "Hungary", 47, 20),
            new Country("IS", "Iceland", 65, -18),
            new Country("IN", "India", 20, 77),
            new Country("ID", "Indonesia", -5, 120),
            new Country("IR", "Iran, Islamic Republic of", 32, 53),
            new Country("IQ", "Iraq", 33, 44),
            new Country("IE", "Ireland", 53, -8),
            new Country("IL", "Israel", 31.5, 34.75),
            new Country("IT", "Italy", 42.8333, 12.8333),
            new Country("JM", "Jamaica", 18.25, -77.5),
            new Country("JP", "Japan", 36, 138),
            new Country("JO", "Jordan", 31, 36),
            new Country("KZ", "Kazakhstan", 48, 68),
            new Country("KE", "Kenya", 1, 38),
            new Country("KI", "Kiribati", 1.4167, 173),
            new Country("KP", "Korea, Democratic People's Republic of", 40, 127),
            new Country("KR", "Korea, Republic of", 37, 127.5),
            new Country("KW", "Kuwait", 29.3375, 47.6581),
            new Country("KG", "Kyrgyzstan", 41, 75),
            new Country("LA", "Lao People's Democratic Republic", 18, 105),
            new Country("LV", "Latvia", 57, 25),
            new Country("LB", "Lebanon", 33.8333, 35.8333),
            new Country("LS", "Lesotho", -29.5, 28.5),
            new Country("LR", "Liberia", 6.5, -9.5),
            new Country("LY", "Libyan Arab Jamahiriya", 25, 17),
            new Country("LI", "Liechtenstein", 47.1667, 9.5333),
            new Country("LT", "Lithuania", 56, 24),
            new Country("LU", "Luxembourg", 49.75, 6.1667),
            new Country("MO", "Macao", 22.1667, 113.55),
            new Country("MK", "Macedonia, The Former Yugoslav Republic of", 41.8333, 22),
            new Country("MG", "Madagascar", -20, 47),
            new Country("MW", "Malawi", -13.5, 34),
            new Country("MY", "Malaysia", 2.5, 112.5),
            new Country("MV", "Maldives", 3.25, 73),
            new Country("ML", "Mali", 17, -4),
            new Country("MT", "Malta", 35.8333, 14.5833),
            new Country("MH", "Marshall Islands", 9, 168),
            new Country("MQ", "Martinique", 14.6667, -61),
            new Country("MR", "Mauritania", 20, -12),
            new Country("MU", "Mauritius", -20.2833, 57.55),
            new Country("YT", "Mayotte", -12.8333, 45.1667),
            new Country("MX", "Mexico", 23, -102),
            new Country("FM", "Micronesia, Federated States of", 6.9167, 158.25),
            new Country("MD", "Moldova, Republic of", 47, 29),
            new Country("MC", "Monaco", 43.7333, 7.4),
            new Country("MN", "Mongolia", 46, 105),
            new Country("ME", "Montenegro", 42, 19),
            new Country("MS", "Montserrat", 16.75, -62.2),
            new Country("MA", "Morocco", 32, -5),
            new Country("MZ", "Mozambique", -18.25, 35),
            new Country("MM", "Myanmar", 22, 98),
            new Country("NA", "Namibia", -22, 17),
            new Country("NR", "Nauru", -0.5333, 166.9167),
            new Country("NP", "Nepal", 28, 84),
            new Country("NL", "Netherlands", 52.5, 5.75),
            new Country("AN", "Netherlands Antilles", 12.25, -68.75),
            new Country("NC", "New Caledonia", -21.5, 165.5),
            new Country("NZ", "New Zealand", -41, 174),
            new Country("NI", "Nicaragua", 13, -85),
            new Country("NE", "Niger", 16, 8),
            new Country("NG", "Nigeria", 10, 8),
            new Country("NU", "Niue", -19.0333, -169.8667),
            new Country("NF", "Norfolk Island", -29.0333, 167.95),
            new Country("MP", "Northern Mariana Islands", 15.2, 145.75),
            new Country("NO", "Norway", 62, 10),
            new Country("OM", "Oman", 21, 57),
            new Country("PK", "Pakistan", 30, 70),
            new Country("PW", "Palau", 7.5, 134.5),
            new Country("PS", "Palestinian Territory, Occupied", 32, 35.25),
            new Country("PA", "Panama", 9, -80),
            new Country("PG", "Papua New Guinea", -6, 147),
            new Country("PY", "Paraguay", -23, -58),
            new Country("PE", "Peru", -10, -76),
            new Country("PH", "Philippines", 13, 122),
            new Country("PL", "Poland", 52, 20),
            new Country("PT", "Portugal", 39.5, -8),
            new Country("PR", "Puerto Rico", 18.25, -66.5),
            new Country("QA", "Qatar", 25.5, 51.25),
            new Country("RE", "Reunion", -21.1, 55.6),
            new Country("RO", "Romania", 46, 25),
            new Country("RU", "Russian Federation", 60, 100),
            new Country("RW", "Rwanda", -2, 30),
            new Country("SH", "Saint Helena", -15.9333, -5.7),
            new Country("KN", "Saint Kitts and Nevis", 17.3333, -62.75),
            new Country("LC", "Saint Lucia", 13.8833, -61.1333),
            new Country("PM", "Saint Pierre and Miquelon", 46.8333, -56.3333),
            new Country("VC", "Saint Vincent and The Grenadines", 13.25, -61.2),
            new Country("WS", "Samoa", -13.5833, -172.3333),
            new Country("SM", "San Marino", 43.7667, 12.4167),
            new Country("ST", "Sao Tome and Principe", 1, 7),
            new Country("SA", "Saudi Arabia", 25, 45),
            new Country("SN", "Senegal", 14, -14),
            new Country("RS", "Serbia", 44, 21),
            new Country("SC", "Seychelles", -4.5833, 55.6667),
            new Country("SL", "Sierra Leone", 8.5, -11.5),
            new Country("SG", "Singapore", 1.3667, 103.8),
            new Country("SK", "Slovakia", 48.6667, 19.5),
            new Country("SI", "Slovenia", 46, 15),
            new Country("SB", "Solomon Islands", -8, 159),
            new Country("SO", "Somalia", 10, 49),
            new Country("ZA", "South Africa", -29, 24),
            new Country("GS", "South Georgia and The South Sandwich Islands", -54.5, -37),
            new Country("ES", "Spain", 40, -4),
            new Country("LK", "Sri Lanka", 7, 81),
            new Country("SD", "Sudan", 15, 30),
            new Country("SR", "Suriname", 4, -56),
            new Country("SJ", "Svalbard and Jan Mayen", 78, 20),
            new Country("SZ", "Swaziland", -26.5, 31.5),
            new Country("SE", "Sweden", 62, 15),
            new Country("CH", "Switzerland", 47, 8),
            new Country("SY", "Syrian Arab Republic", 35, 38),
            new Country("TW", "Taiwan, Province of China", 23.5, 121),
            new Country("TJ", "Tajikistan", 39, 71),
            new Country("TZ", "Tanzania, United Republic of", -6, 35),
            new Country("TH", "Thailand", 15, 100),
            new Country("TG", "Togo", 8, 1.1667),
            new Country("TK", "Tokelau", -9, -172),
            new Country("TO", "Tonga", -20, -175),
            new Country("TT", "Trinidad and Tobago", 11, -61),
            new Country("TN", "Tunisia", 34, 9),
            new Country("TR", "Turkey", 39, 35),
            new Country("TM", "Turkmenistan", 40, 60),
            new Country("TC", "Turks and Caicos Islands", 21.75, -71.5833),
            new Country("TV", "Tuvalu", -8, 178),
            new Country("UG", "Uganda", 1, 32),
            new Country("UA", "Ukraine", 49, 32),
            new Country("AE", "United Arab Emirates", 24, 54),
            new Country("GB", "United Kingdom", 54, -2),
            new Country("US", "United States", 38, -97),
            new Country("UM", "United States Minor Outlying Islands", 19.2833, 166.6),
            new Country("UY", "Uruguay", -33, -56),
            new Country("UZ", "Uzbekistan", 41, 64),
            new Country("VU", "Vanuatu", -16, 167),
            new Country("VE", "Venezuela", 8, -66),
            new Country("VN", "Viet Nam", 16, 106),
            new Country("VG", "Virgin Islands, British", 18.5, -64.5),
            new Country("VI", "Virgin Islands, U.S.", 18.3333, -64.8333),
            new Country("WF", "Wallis and Futuna", -13.3, -176.2),
            new Country("EH", "Western Sahara", 24.5, -13),
            new Country("YE", "Yemen", 15, 48),
            new Country("ZM", "Zambia", -15, 30),
            new Country("ZW", "Zimbabwe", -20, 30), 
        };
    }
}