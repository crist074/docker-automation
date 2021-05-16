# QA Automation Example
 
CI/CD Demonstration with GitHub and Docker deployment to Heroku.

Test execution performed by dotnet, Selenium, and NUnit.

# How to Run tests manually
1. Clone repository to local
2. Navigate to `/tests`
3. Run `dotnet test` in powershell
4. Results will be placed in `/Reports`

# Test Automation
Tests are also run automatically through GitHub actions. (https://github.com/crist074/docker-automation/actions)

Upon code commit to the repo:
1. Web app is rebuilt and deployed to Heroku (https://the-internet-automation.herokuapp.com/)
2. ```dotnet test``` is run against a combination of production code and the deployed app

# UITests.cs
1. ParameterizedTest();
2. LargeDOM();
3. VerifyInsuranceFilter(string nameOfInsurance, string zipCode);
4. LoginAndLogout();
5. FailedLogin_IncorrectUsername();
6. FailedLogin_IncorrectPassword();

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
```/tests``` contains the VSCode solution and workspace if you want to edit the code. XML documentation of each test case is also included in the .cs files.

https://code.visualstudio.com/

# Dotnet sdk
https://dotnet.microsoft.com/download
