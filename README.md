# BlackTrader (Albion Trade Analyser)
BlackTrader is a project to Compare Albion Online city prices bases on Albion online project.

  - Auto offer best benefits for trading between cities.
  - Base on [Albion Online Data Project](https://www.albion-online-data.com)

## New Features!

  - get prices by a command. And view items date difference to now.
  - get trade offers from special city.
  - you can sell your items to best places or buy them with lower prices.


You can also:
  - Save and Load items data.

### Installation

Download releases or build source code. Im using continuous deployment.
Please install and use [Albion Online Data Project client](https://github.com/broderickhyman/albiondata-client), this client project updating in game store data.


#### Attention
if you are using this project, some shop data may be old and its depend on [Albion Online Data Project](https://www.albion-online-data.com) you can help us to set new data by installing [Albion Online Data Project client](https://github.com/broderickhyman/albiondata-client) before BlackTrader Project.
We are not accept any responsibility for the loss of your money,silvers or golds at the purchases of the game. Attention to offer dates before buying itmes or selling them.
#### Is This Allowed
'Our position is quite simple. As long as you just look and analyze we are ok with it. The moment you modify or manipulate something or somehow interfere with our services we will react (e.g. perma-ban, take legal action, whatever).'
~MadDave, Technical Lead at Sandbox Interactive for Albion Online, source
### Using
There two way to use, click on BlackTrader.exe and run it or open Windows Power shell or CMD at App Folder and call ./BlackTrader.exe.
you can see "Please enter a command:" ,after oppening the app.
you can see list of commands by entering  
```Cmd
Help
```
at this Application you have Item DataPool that can store your received items data from Albion Online Data Project. this is help you to analyse multiple time without ReDownloading data. and you can save or load received data to files on your disk. you can update and redownload items data. item data contains buy and sell prices for multiple cities. attention to commands list.
#### Commands
* Exit, End to exit from.
* Search [part of ID] to get list of items IDs that contain [part of ID]
* AddSearch [part of ID] to add list of items that contain [part of ID] to DataPool. its take long time to download items data.
* UpdateSearch [part of ID] to update list of items that contain [part of ID] at your DataPool. its take long time to download items data.
* City to get list of recived city names from Data Project.
* Add [Item ID] to add an item to data pool.
* Remove [Item ID] to remove an item from data pool.
* Items to get stored items of the data pool.
* Save to save on auto save file.
* Save [File Name] to save DataPool on your disk.
* Load for load auto save file.
* Load [File Name] to load DataPool from your disk.
* Update to Update all items of data pool. its take long time, if you have so many items.
* Update [Item ID] to update only [Item ID] at data pool.
* TradeOffers to analyse data pool and get offers for a good trade at game.
* TradeOffers [City Name] to get offers from [City Name].
* TradeOffers [From City Name] [To City Name] to get offers from [City Name] to [From City Name].
* WhereSell [ItemID] to see the best places to sell an item.
* WhereBuy [ItemID] to see the best places to buy an item.
* Conf to change config analysis app

#### Example Trade Offer:
```
* you can buy T6_ARMOR_CLOTH_KEEPER@2 from Thetford for 195427 (1 Hours Ago) and sell to Black Market for 196196 (2 Hourse Ago) then Profit: 769
```
### Todos

 - More analysis commands.
 - Stable version.
 - Better UI.

License
----

MIT


**Free Software, Hell Yeah!**
