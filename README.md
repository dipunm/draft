STRUCTURE:
Base
- Contains all Web endpoints to the solution. May contain console applications, scheduled services, etc. in the future.

Domain
- Contains all the 'features' of the site. In a crud application, it will contain only models. All features start here.

Infrastructure
- Contains all the concrete implementations of the domain features and its dependencies.

PROJECTS
==========

Base:
- The shell projects for the domain logic.
- Contains but is not limited to web applications.
- Each shell application is responsible for pairing the appropriate domain services with their implementations found in Infrastructure.
- May contain small amounts of Infrastructure/bootstrap code related to its core technology (eg. IIS or Quartz)

Domain:
- Too many projects can be bad. Don't go overboard.
- Make projects by feature, not by technology. (eg. ProductSearch vs Search)
- Domain tends to contain interfaces, models, and services with business logic but should avoid technology coupling where possible.
- As soon as something is likely to be modified to use a new technology, move the concrete services to a new Infrastructure project.
- As soon as multiple features share a technology, move the concrete services to a new Infrastructure project.

Infrastructure:
- These projects manage cross cutting concerns and are usually transparent to Domain.
- Make projects by technology and/or version of technology, not by feature. (eg. Solr can provide for Searchers, Caches, Repositories etc.)
- Infrastructure should reference Domain, not the other way around.
- Configuration models get defined alongside concrete classes that require them, but should be technology agnostic like domain models.
- Configuration follows the same rules as other Infrastructure; one project per technology (eg. custom configuration sections vs database vs json files etc.).

TESTS:
- Tests sit next to their projects. This makes it easy to track which project has tests and where to put/find tests for new functionality.



+---------------+---------------+----------------+
|               |               |                |
|  Shopomo.Web  |  Shopomo.Api  |  Shopomo.Cron  |
|               |               |				 |
+------------------------------------------------+
| Product Search | Curated Pages | Curated Sects |
| Departments hierarchy | Curated Landing Pages  |
| Cache Managing | Brand Listing | Autocomplete  |
|                    etc...                      |
+------------------------------------------------+
|  Solr | Mongo | Redis | Log4Net + | Wordpress  |
+------------------------------------------------+


DOWNSIDES:
============

---- HOW TO DETECT AND DEPLOY ONLY AFFECTED PROJECTS? --- we don't. We deploy conciously
---- OPTIMISING Infrastructure code without affecting Domain design?
---- Shared code in Domain projects?