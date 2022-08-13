# MilkingMoney [Cloud Computing Capstone Project]

### What question is this solving? 
As Professor Giordano mentioned in the recitation, the farm collects a large sum of data about each of the cows. While these data points seem somewhat random, they contain important information that can reflect the monetary value of a cow. This not only allows the farmers to quickly identify the financial standing of the farms, it can also predict how the farms will do in the future with the current trend. This enables the farmers to make adjustments accordingly. 

### What are the application scenarios of your system?
The architecture of the solution system connects the many sensors scattered across the different farms to a centralized hub which is processed and analyzed and ultimately displayed to the user in a digestible and interactive form. While the project was designed with cow sensor data in mind, the architecture can be generalized to make sense from any seemingly random and scattered data. The system collects and synthesizes meaningful information with the help of regression machine learning models that are trained and retrained based on the collected data.

### How is it using the cloud computing resources to solve the problem?
We are trying to collect information from farms scattered across remote areas. Traditionally, we would either have to set up our infrastructure near the farms, or we would have to suffer from poor latency and connectivity, with our dashboard diverging significantly from realtime and have missing data as well. Cloud computing resources help to solve this problem. Lightweight Azure IoT edge instances help to process, validate and relay information to a centralized IoT hub for further analysis. This drastically reduces the cost of having to set up the entirety of the computing infrastructure near the farm, while still retaining the low latency that comes with having the edge functions deployed near the farm. 

With the Azure IoT hub, the system can easily scale when new cows and thus sensors are added; we simply need to spin up new IoT edge instances which can be handled automatically by the IoT hub. The end-to-end data flow does not change at all.

Finally, the system is self-sufficient since the model will retrain based on collected data; little maintenance is required. The exact architecture is discussed below.

### Project Demo
[![Demo](http://img.youtube.com/vi/l0kV47VgsDQ/0.jpg)](http://www.youtube.com/watch?v=l0kV47VgsDQ "CS 5412 Final Project Presentation")
