using System;
using ActivityReservation.WechatAPI.Entities;
using Microsoft.Extensions.Caching.Memory;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    public static class WechatTokenHelper
    {
        /// <summary>
        /// GetAccessTokenUrlFormat
        /// 0:appid
        /// 1:secret
        /// </summary>
        private const string GetAccessTokenUrlFormat = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 获取公共号 AccessToken
        /// </summary>
        /// <returns>AccessToken</returns>
        public static string GetAccessToken()
        {
            var token = DependencyResolver.Current.ResolveService<IMemoryCache>().GetOrCreate("wechat_access_token",
                (entry) =>
                {
                    var value = RetryHelper.TryInvoke(() => HttpHelper.HttpGetFor<AccessTokenEntity>(
                            GetAccessTokenUrlFormat.FormatWith(WeChatConsts.AppId, WeChatConsts.AppSecret)),
                        result => result.AccessToken.IsNotNullOrWhiteSpace());
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(7120);
                    return value;
                });
            return token?.AccessToken;
        }
    }
}
