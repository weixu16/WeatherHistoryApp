# WeatherHistoryApp
This web app can help get the history weather information of cities in the U.S. If it is already been searched before, it will show the history results. If not, the city will be added to the monitoring list. 


## Configuration Details:

Server side: 
1. In CityWeather.cs file, the appId should be changed to user's own appId. It can be get from http://api.openweathermap.org
2. In web.config file, please upload your own storageaccount connection string in appsetting section:    
      \<add key="StorageConnectionString" value="" /\>
3. Open vs project file, you can start project locally or deploy to the azure app service. 

Client side: 
1. Install npm, and run "npm install" under the client folder. 
2. Run "npm start" to boot up the app locally. 


