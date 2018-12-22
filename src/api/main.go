package main

func main() {
	// Create a new instance of Echo
	e := InitRouter()

	// Start as a web server
	e.Start(":999")
}
