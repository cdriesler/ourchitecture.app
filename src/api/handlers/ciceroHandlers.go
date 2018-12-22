package handlers

import (
	"net/http"

	"github.com/labstack/echo"
)

func GetTasks() echo.HandlerFunc {
	return func(c echo.Context) error {
		return c.JSON(http.StatusOK, "tasks")
	}
}
