[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Pulls][pulls-shield]][pulls-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">

  <h1 align="center">SocialKnow </h1>

  <p align="center">
    Social media platform with knowledge base and services for a given industry.
    <br />
    <br />
    <a href="https://github.com/PolishDevCom/SocialKnow/issues">Report Bug</a>
    ·
    <a href="https://github.com/PolishDevCom/SocialKnow/issues">Request Feature</a>
  </p>
</p>

<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
* [Used Technologies](#used-technologies)
* [Current State of Project](#current-state-of-project)
* [Getting Started](#getting-started)
* [Usage](#usage)
* [Contributing](#contributing)
* [Contact](#contact)



<!-- ABOUT THE PROJECT -->
## About The Project

It is a web application project that is a social networking site with a knowledge base. It is not supposed to be another Facebook, but a web application designed to gather people interested in a specific field and provide them with functions such as:
* knowledge base,
* the ability to create lists and evaluate specialists in this field,
* a list of proven specialist stores,
* forum - with moderating options,
* internal communicator,
* section with events - information about events related to a given topic,
* place for legal notes and regulations (portal regulations, GDPR, privacy policy).

<!-- USED TECHNOLOGIES -->
### Used Technologies

The application has been clearly divided into the backend and frontend parts. The backend is implemented using .NET Core and the frontend is carried out using the React library.

Backend
* [ASP.NET Core 3.1](https://docs.microsoft.com/pl-pl/aspnet/core/?view=aspnetcore-3.1)
* [PostgreSQL](https://www.postgresql.org/)
* [Entity Framework Core](https://docs.microsoft.com/en-US/ef/core/)
* [ASP.NET Core Identity](https://docs.microsoft.com/en-US/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio)
* [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://fluentvalidation.net/)
* [AutoMapper](https://automapper.org/)
* [CloudinaryDotNet](https://cloudinary.com/)
* [Swashbuckle](https://docs.microsoft.com/en-US/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio)
* [NUnit](https://nunit.org/)
* [Moq](https://github.com/Moq/moq4/wiki/Quickstart)
* [Bogus](https://github.com/bchavez/Bogus)
* [FluentAssertion](https://fluentassertions.com/)

Frontend
* [React](https://reactjs.org/)
* [Redux](https://redux.js.org/)
* [JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript)
* [HTML](https://developer.mozilla.org/en-US/docs/Web/HTML)
* [CSS](https://developer.mozilla.org/en-US/docs/Learn/Getting_started_with_the_web/CSS_basics)

<!-- CURRENT STATE OF THE PROJECT -->
## Current State of Project

Currently, an MVP (Minimal Value Project) is being created, where the backend already has part of the implementation, while the creation of the React project and its integration are in progress.

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps:

* Download the latest stable version from the download tab and unzip it to your folder
* Open the solution in Visual Studio 2019. 
* Clean solution.
* In `appsettings.json` change PostgreSQL DB connection string:

```json
"ConnectionStrings": 
{
    "DefaultConnection": "yourDatabaseConnectionString"
}
```
* For SK.API project set user secrets using CLI or directly in `secrets.json`:

```json
{
  "TokenKey": "yourSecretKeyForAuthorization",
  "Cloudinary:CloudName": "yourCloudinaryCloudName",
  "Cloudinary:ApiSecret": "yourCloudinaryApiSecret",
  "Cloudinary:ApiKey": "yourCloudinaryApiKey"
}
```
* Build the solution.
* Run application
* If you want to use SwaggerUI fire up your browser and open url `http://localhost:5000/`
* Enjoy ;-)

Please note that the app was tested in Chrome browser where no issues where discovered.

<!-- USAGE EXAMPLES -->
## Usage

For usage instruction check project's Wiki: [https://github.com/PolishDevCom/SocialKnow/wiki](https://github.com/PolishDevCom/SocialKnow/wiki)

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. 🍴 Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. 🔃 Open a Pull Request


<!-- CONTACT -->
## Contact

* Project Link: [https://github.com/PolishDevCom/SocialKnow](https://github.com/PolishDevCom/SocialKnow)
* Polishdev Team #1: [https://www.polishdev.com/](https://www.polishdev.com/?p=56)

### Project's main contributors:

<a href="https://github.com/PolishDevCom/SocialKnow/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=PolishDevCom/SocialKnow" />
</a>

<!-- Made with [contributors-img](https://contributors-img.web.app). -->

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/UgzSourceCode/SocialKnow.svg?style=flat-square
[contributors-url]: https://github.com/UgzSourceCode/SocialKnow/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/UgzSourceCode/SocialKnow.svg?style=flat-square
[forks-url]: https://github.com/UgzSourceCode/SocialKnow/network/members
[stars-shield]: https://img.shields.io/github/stars/UgzSourceCode/SocialKnow.svg?style=flat-square
[stars-url]: https://github.com/UgzSourceCode/SocialKnow/stargazers
[issues-shield]: https://img.shields.io/github/issues/UgzSourceCode/SocialKnow.svg?style=flat-square
[issues-url]: https://github.com/UgzSourceCode/SocialKnow/issues
[pulls-shield]: https://img.shields.io/github/issues-pr/UgzSourceCode/SocialKnow.svg?style=flat-square
[pulls-url]: https://github.com/UgzSourceCode/SocialKnow/pulls
