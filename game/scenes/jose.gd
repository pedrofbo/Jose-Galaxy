extends KinematicBody

var speed = 500
var direction = Vector3()
var gravity = -30
var velocity = Vector3()
var planet
var planet_num = 0
var up_direction = Vector3()
var gravity_up = Vector3()
var prev_input




# Called when the node enters the scene tree for the first time.
func _ready():
	planet = get_parent().get_node("Plataforma")
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	var t = Transform()

	if planet_num == 1:
		t.basis.x = Vector3(1, 0, 0)
		t.basis.y = Vector3(0, 1, 0)
		t.basis.z = Vector3(0, 0, 1)
		t.origin = transform.origin

		transform = t.orthonormalized()
		planet_num = 0
	elif planet_num == 2:
		t.basis.x = Vector3(1, 0, 0)
		t.basis.y = Vector3(0, 0, -1)
		t.basis.z = Vector3(0, 1, 0)
		t.origin = transform.origin

		transform = t.orthonormalized()
		planet_num = 0
	elif planet_num == 3:
		t = transform
		look_at(planet.get_global_transform().origin, transform.basis.y)
		transform.basis.y = transform.basis.z.normalized()
		transform.basis.x = t.basis.x
		transform.basis.z = transform.basis.x.cross(transform.basis.y).normalized()
		if prev_input == 2:
			transform.basis.z = t.basis.z
			transform.basis.x = transform.basis.y.cross(transform.basis.z).normalized()
#		transform.basis.z = t.basis.x
#		transform.basis.x = t.basis.y
#		planet_num = 0
	
	velocity = transform.basis.inverse() * velocity

	up_direction = transform.basis.y
	
	direction = Vector3(0, 0, 0)
	if Input.is_action_pressed("move_fw"):
		direction.z += 1
		prev_input = 1
	if Input.is_action_pressed("move_back"):
		direction.z += -1
		prev_input = 1
	if Input.is_action_pressed("move_left"):
		direction.x += 1
		prev_input = 2
	if Input.is_action_pressed("move_right"):
		direction.x += -1
		prev_input = 2
	
	direction = direction.normalized()
	direction = direction * speed * delta
	
	velocity.y += gravity * delta
	velocity.x = direction.x
	velocity.z = direction.z
	
#	velocity = direction
#	velocity += gravity * delta * up_direction
#	if is_on_floor() and Input.is_key_pressed(KEY_SPACE):
#			velocity += 400 * up_direction
	if is_on_floor() and Input.is_key_pressed(KEY_SPACE):
		velocity.y = 20

	velocity = transform.basis * velocity
	velocity = move_and_slide(velocity, up_direction)

#	if is_on_floor() and Input.is_key_pressed(KEY_SPACE):
#		velocity.y = 20
	

func _on_Area_body_entered(body):
	if not is_on_floor():
		planet = get_parent().get_node("Planeta")
		print("yo")
		planet_num = 3
		print(up_direction)


func _on_Area_body_entered_plat(body):
	if not is_on_floor():
		planet = get_parent().get_node("Plataforma")
		print("yoho")
		planet_num = 1
		print(up_direction)


func _on_Area_body_entered_plat2(body):
	if not is_on_floor():
		planet = get_parent().get_node("Plataforma3")
		print("yohoho")
		planet_num = 2
		print(up_direction)


func _on_Area_body_entered_planeta2(body):
	if not is_on_floor():
		planet = get_parent().get_node("Planeta2")
		print("yo")
		planet_num = 3
		print(up_direction)
