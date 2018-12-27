package handlers

import (
	"net/http"

	"github.com/labstack/echo"
)

func GetVersion() echo.HandlerFunc {
	return func(c echo.Context) error {
		return c.JSON(http.StatusOK, "0.0.1")
	}
}
