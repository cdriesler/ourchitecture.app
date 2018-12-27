package main

import (
	"github.com/cdriesler/ourchitecture.api/handlers"
	"github.com/labstack/echo"
	"github.com/labstack/echo/middleware"
)

// Route contains pattern and function for all requests. Type is determined by container.
type Route struct {
	Name        string
	Pattern     string
	HandlerFunc echo.HandlerFunc
}

// GetRoutes is a slice of Route structs for GET requests.
type GetRoutes []Route

// PostRoutes is a slice of Route structs for POST requests.
type PostRoutes []Route

// InitRouter is called at the start of the function and returns the router with routes.
func InitRouter() *echo.Echo {

	currentVersion := "/api/v1"

	e := echo.New()

	e.Pre(middleware.RemoveTrailingSlash())

	e.Use(middleware.Static("../../dist/boxboxbox/"))
	e.File("/", "../../dist/boxboxbox/index.html")

	e.Pre(middleware.Rewrite(map[string]string{
		"/api/version": currentVersion + "/version",
	}))

	for _, route := range getRoutes {
		e.GET(currentVersion+route.Pattern, route.HandlerFunc)
	}

	for _, route := range postRoutes {
		e.POST(currentVersion+route.Pattern, route.HandlerFunc)
	}

	return e
}

var getRoutes = GetRoutes{
	Route{
		"Version",
		"/version",
		handlers.GetVersion(),
	},
}

var postRoutes = PostRoutes{
	Route{
		"Cicero-Main",
		"/cicero",
		handlers.CiceroTest(),
	},
}
