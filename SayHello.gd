extends Panel

var timer = 0

func _ready():
#	get_node("Button").connect("pressed", self, "_on_Button_pressed")
	pass

func _process(delta):
	var oldTime = timer
	timer += delta
	if (round(timer) > oldTime):
		get_node("Label").text = str(round(timer))

#func _on_Button_pressed():
#	get_node("Label").text = "HELLO!"
