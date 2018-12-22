package main

import (
	"github.com/cdriesler/ourchitecture.api/handlers"
	"github.com/labstack/echo"
	"github.com/labstack/echo/middleware"
)

type GetRoute struct {
	Name        string
	Pattern     string
	HandlerFunc echo.HandlerFunc
}

type GetRoutes []GetRoute

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

	return e
}

var getRoutes = GetRoutes{
	GetRoute{
		"Version",
		"/version",
		handlers.GetVersion(),
	},
}
