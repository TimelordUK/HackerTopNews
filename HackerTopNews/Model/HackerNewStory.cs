﻿namespace HackerTopNews.Model
{
    /*
      "by": "dmotz",
    "descendants": 1595,
    "id": 38544729,
    "kids": [
        38547486,
        38547323,
        38545663,
        38545430,
        38545113,
        38545203,
        38546464,
        38546562,
        38548351,
        38545012,
        38545010,
        38545044,
        38546054,
        38546046,
        38546447,
        38545245,
        38544877,
        38544957,
        38552471,
        38545654,
        38546159,
        38546006,
        38545429,
        38545220,
        38545081,
        38545068,
        38545018,
        38546384,
        38544821,
        38548739,
        38552399,
        38545050,
        38546963,
        38544997,
        38545326,
        38545551,
        38545848,
        38551570,
        38546344,
        38545073,
        38545375,
        38545810,
        38546275,
        38545669,
        38547434,
        38545007,
        38547079,
        38547414,
        38544994,
        38546388,
        38545103,
        38547387,
        38546477,
        38545057,
        38545982,
        38544940,
        38551526,
        38546463,
        38547392,
        38547791,
        38544941,
        38577442,
        38551124,
        38546036,
        38544863,
        38547467,
        38546113,
        38549166,
        38549425,
        38546241,
        38553019,
        38555702,
        38549953,
        38548294,
        38545516,
        38547962,
        38546590,
        38545129,
        38551886,
        38557554,
        38545168,
        38549442,
        38546138,
        38547109,
        38549749,
        38551880,
        38545030,
        38550681,
        38553828,
        38546566,
        38546559,
        38545095,
        38547365,
        38547425,
        38555793,
        38546256,
        38548427,
        38544969,
        38546095,
        38550613,
        38545609,
        38546356,
        38547001,
        38544951,
        38547054,
        38558903,
        38550081,
        38551539,
        38547204,
        38547013,
        38552494,
        38551532,
        38548056,
        38549689,
        38548052,
        38560497,
        38548406,
        38547362,
        38547028,
        38545713,
        38545971,
        38545991,
        38551947,
        38554111,
        38544896,
        38552030,
        38547019,
        38548608,
        38545069,
        38563846,
        38545955,
        38549601,
        38562943,
        38547222,
        38549712,
        38547661,
        38547662,
        38547056,
        38553366,
        38553391,
        38550609,
        38552978,
        38545889,
        38552338,
        38548482,
        38550986,
        38548411,
        38565201,
        38545572,
        38545161,
        38548272,
        38569048,
        38544939,
        38550713,
        38549170,
        38544929,
        38545466,
        38545389,
        38566998,
        38546226,
        38547521,
        38547145,
        38551110,
        38550710,
        38550927,
        38547029,
        38545898,
        38547301,
        38546428,
        38548889,
        38546405,
        38545179,
        38547443,
        38545233,
        38545254,
        38545089,
        38551117,
        38546928,
        38549288,
        38547217,
        38552557,
        38545172,
        38546719,
        38568337,
        38545330,
        38545750,
        38548386,
        38547052,
        38548884,
        38555320,
        38545494,
        38547672,
        38547415,
        38552180,
        38547173,
        38547494,
        38552394,
        38551814,
        38549870,
        38548177,
        38548757,
        38547678,
        38551408,
        38551046,
        38545082,
        38558034,
        38552840,
        38551697,
        38545311,
        38548621,
        38549473,
        38544989,
        38547800,
        38550367,
        38547773,
        38548641,
        38546084,
        38546064,
        38551508,
        38545127,
        38551079,
        38544943,
        38549533,
        38546631,
        38545009,
        38546379,
        38549028,
        38545021,
        38551387,
        38552170,
        38552633,
        38553821,
        38547185,
        38556537,
        38550158,
        38545231,
        38545604,
        38548693,
        38546937,
        38545403,
        38553105,
        38544901,
        38545325,
        38545123,
        38545159,
        38547519,
        38575382,
        38545932,
        38558117,
        38548796,
        38552726,
        38548670,
        38547044,
        38544831,
        38556149,
        38545465,
        38547464,
        38547465,
        38546775,
        38545801,
        38545001,
        38548065,
        38563392,
        38551474,
        38545058
    ],
    "score": 2121,
    "time": 1701875027,
    "title": "Gemini AI",
    "type": "story",
    "url": "https://deepmind.google/technologies/gemini/"
    */
    public class HackerNewStory
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public long Time { get; set; }
        public int Score { get; set; }
        public int Id { get; set; }
        public string By { get; set; }
        public List<int> Kids { get; set; }
        public override string ToString()
        {
            return $"HackerNewStory Url = {Url}, Type = {Type}, Title = '{Title}', Time = {Time}, Score = {Score}, Id = {Id}, By = {By}, Kids = {(Kids != null ? string.Join(",", Kids) : "")}";
        }
    }
}
