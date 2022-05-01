# RealEstate
Funda's Real Estate Agents

.In order to use application, RealEstateApiKey, HttpRequestInitialCount and must be provided in appsettings.json like:

      {
        "RealEstateApiKey": "key",
        "HttpRequestInitialCount": 10,
        "HttpRequestMaxCount":  10
      }

.There are two different endpoints. One is for listing top 10 real estate agents,
the other one is for listing top 10 real estate agents with garden.

.Swagger is integrated. Endpoints can be used by it. 
https://localhost:5001/swagger/index.html
