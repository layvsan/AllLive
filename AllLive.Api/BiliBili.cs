﻿using AllLive.Core.Interface;
using AllLive.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AllLive.Core.Danmaku;
using AllLive.Core.Helper;
using Newtonsoft.Json.Linq;

namespace AllLive.Core
{

    public class BiliBili :ILiveSite
    {
        public string Name => "哔哩哔哩直播";
        public ILiveDanmaku GetDanmaku() => new BiliBiliDanmaku();
        public async Task<List<LiveCategory>> GetCategores()
        {
            List<LiveCategory> categories = new List<LiveCategory>();
            var result =await HttpUtil.GetString("https://api.live.bilibili.com/room/v1/Area/getList?need_entrance=1&parent_id=0");
            var obj = JObject.Parse(result);
            foreach (var item in obj["data"])
            {
                List<LiveSubCategory> subs = new List<LiveSubCategory>();
                foreach (var subItem in item["list"])
                {
                    subs.Add(new LiveSubCategory() { 
                        Pic= subItem["pic"].ToString(),
                        ID= subItem["id"].ToString(),
                        ParentID= subItem["parent_id"].ToString(),
                        Name =subItem["name"].ToString(),
                    });
                }

                categories.Add(new LiveCategory() { 
                    Children=subs,
                    ID=item["id"].ToString(),
                    Name= item["name"].ToString(),
                });
            }
            return categories;
        }
        public async Task<LiveCategoryResult> GetCategoryRooms(LiveSubCategory category, int page = 1)
        {
            LiveCategoryResult categoryResult = new LiveCategoryResult() { 
                Rooms=new List<LiveRoomItem>(),
               
            };
            var result = await HttpUtil.GetString($"https://api.live.bilibili.com/xlive/web-interface/v1/second/getList?platform=web&parent_area_id={category.ParentID}&area_id={category.ID}&sort_type=&page={page}");
            var obj = JObject.Parse(result);
            categoryResult.HasMore = (int)obj["data"]["has_more"] == 1;
            foreach (var item in obj["data"]["list"])
            {
                categoryResult.Rooms.Add(new LiveRoomItem() { 
                    Cover= item["cover"].ToString(),
                    Online= (int)item["online"],
                    RoomID= item["roomid"].ToString(),
                    Title = item["title"].ToString(),
                    UserName = item["uname"].ToString(),
                });
            }
            return categoryResult;
        }
        public async Task<LiveCategoryResult> GetRecommendRooms(int page = 1)
        {
            LiveCategoryResult categoryResult = new LiveCategoryResult()
            {
                Rooms = new List<LiveRoomItem>(),

            };
            var result = await HttpUtil.GetString($"https://api.live.bilibili.com/room/v1/Area/getListByAreaID?areaId=0&sort=online&pageSize=30&page={page}");
            var obj = JObject.Parse(result);
            categoryResult.HasMore = ((JArray)obj["data"]).Count > 0;
            foreach (var item in obj["data"])
            {
                categoryResult.Rooms.Add(new LiveRoomItem()
                {
                    Cover = item["cover"].ToString(),
                    Online = (int)item["online"],
                    RoomID = item["roomid"].ToString(),
                    Title = item["title"].ToString(),
                    UserName = item["uname"].ToString(),
                });
            }
            return categoryResult;
        }
        public async Task<LiveRoomDetail> GetRoomDetail(LiveRoomItem room)
        {
            var result = await HttpUtil.GetString($"https://api.live.bilibili.com/xlive/web-room/v1/index/getH5InfoByRoom?room_id={room.RoomID}");
            var obj = JObject.Parse(result);

            return new LiveRoomDetail()
            {
                Cover = obj["data"]["room_info"]["cover"].ToString(),
                Online = (int)obj["data"]["room_info"]["online"],
                RoomID = obj["data"]["room_info"]["room_id"].ToString(),
                Title = obj["data"]["room_info"]["title"].ToString(),
                UserName = obj["data"]["anchor_info"]["base_info"]["uname"].ToString(),
                Introduction= obj["data"]["room_info"]["description"].ToString(),
                Notice = "",
                Status = (int)obj["data"]["room_info"]["live_status"] ==1,
                DanmakuData= (int)obj["data"]["room_info"]["room_id"]
            };
        }
        public async Task<LiveSearchResult> Search(string keyword, int page = 1)
        {
            LiveSearchResult searchResult = new LiveSearchResult()
            {
                Rooms = new List<LiveRoomItem>(),

            };
            var result = await HttpUtil.GetString($"https://api.bilibili.com/x/web-interface/search/type?context=&search_type=live&cover_type=user_cover&page={page}&order=&keyword={Uri.EscapeDataString(keyword)}&category_id=&__refresh__=true&_extra=&highlight=0&single_column=0");
            var obj = JObject.Parse(result);
           
            foreach (var item in obj["data"]["result"]["live_room"])
            {
                searchResult.Rooms.Add(new LiveRoomItem()
                {
                    Cover = "https:"+item["cover"].ToString(),
                    Online = (int)item["online"],
                    RoomID = item["roomid"].ToString(),
                    Title = item["title"].ToString(),
                    UserName = item["uname"].ToString(),
                });
            }
            searchResult.HasMore = ((JArray)obj["data"]["result"]["live_room"]).Count > 0;
            return searchResult;
        }
        public async Task<List<LivePlayQuality>> GetPlayQuality(LiveRoomDetail roomDetail)
        {
            List<LivePlayQuality> qualities = new List<LivePlayQuality>();
            var result = await HttpUtil.GetString($"https://api.live.bilibili.com/room/v1/Room/playUrl?cid={roomDetail.RoomID}&qn=&platform=web");
            var obj = JObject.Parse(result);
            foreach (var item in obj["data"]["quality_description"])
            {
                qualities.Add(new LivePlayQuality() { 
                    Quality= item["desc"].ToString(),
                    Data = (int)item["qn"],
                });
            }
            return qualities;
        }
        public async Task<List<string>> GetPlayUrls(LiveRoomDetail roomDetail, LivePlayQuality qn)
        {
            List<string> urls = new List<string>();
            var result = await HttpUtil.GetString($"https://api.live.bilibili.com/room/v1/Room/playUrl?cid={roomDetail.RoomID}&qn={qn.Data}&platform=web");
            var obj = JObject.Parse(result);
            foreach (var item in obj["data"]["durl"])
            {
                urls.Add(item["url"].ToString());
            }
            return urls;
        }
    }
}
