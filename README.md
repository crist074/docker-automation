# QA Automation Example
 
CI/CD Demonstration with GitHub and Docker deployment to Heroku.

Test execution performed by dotnet, Selenium, and NUnit.

# How to Run tests
1. Clone repository to local
2. Navigate to `/tests`
3. Run `dotnet test` in powershell
4. Results will be placed in `/Reports`

# UITests.cs
1. ParameterizedTest();
2. LargeDOM();
3. VerifyInsuranceFilter(string nameOfInsurance, string zipCode);

# APITests.cs
1. ZipCodeValidator(APITestCaseSource testCase);
2. JsonDeserialization();
3. GetRandomAPI();
4. GetHealth();
5. CatFact()
6. POST();
7. DELETE();

# Reporting Suite
Reports are generated automatically and are uploaded as GitHub actions artifacts on each build.
https://www.extentreports.com/docs/versions/4/net/index.html

# Docker deployed app
https://the-internet-automation.herokuapp.com/
(clone of https://hub.docker.com/r/gprestes/the-internet/)

# Public APIs
https://github.com/public-apis/public-apis

# Visual Studio Code
https://code.visualstudio.com/

# Dotnet sdk
https://dotnet.microsoft.com/download
