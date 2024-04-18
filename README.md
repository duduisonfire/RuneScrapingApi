# RuneScrapingApi

This program is an API to get the best League of Legends champion runes according to u.gg.
If you are making an League of Legends app you can use this API to get the runes formatted to use in Lol Client Api.

# Disclaimer
Due to the fact that riot forces vanguard and LoL will probably stop working on Linux, this project has lost its meaning for me, I leave here my protests against riot. This implies that the server that hosted this API has been shut down, you can still use the code and host it if you want.

# Explanation of how it works
This API consists of a controller that receives the champion and the lane to be played and returns the set of runes.

The controller starts by checking the mongo database if the runes are cached, if there is a cache it returns directly to the API return.

If the requested set of runes is not cached in the database, the controller invokes the RuneWebScrap class and passes the champion and lane as the instantiation parameter, this class makes a scrap on the u.gg website of the champion's page and parses the runes.

With the list of runes used by the champion, the program invokes the RunePage class to assemble the rune page and parse the name to the rune IDs.

Once this is done, the controller invokes the RuneResponse class to assemble the request return, save it in the database and finally return it to the end-point.

Another functionality implemented is that when the program starts it checks in the database if all the runes of all champions are saved in the database, if not, it caches the runes in the database.

It also has a cronjob scheduled to repeat this process at 2 am.

# Test It
You can test and uses this api making an get requisition to https://runemaker.igorcoder.tech/swagger/index.html : https://runemaker.igorcoder.tech/api/ugg/{champion}/{lane}
