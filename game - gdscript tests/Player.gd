extends KinematicBody

var speed = 500
var direction = Vector3()
var gravity = -30
var velocity = Vector3()
var camera
var planet
var planet_num = 0
var up_direction = Vector3()
var gravity_up = Vector3()
var prev_input

var character


#func transformar_base(tr, vetor):
#	var x = tr.inverse().basis.x.dot(vetor)
#	var y = tr.inverse().basis.y.dot(vetor)
#	var z = tr.inverse().basis.z.dot(vetor)
#
#	return Vector3(x, y, z)
#
#func rotate_basis(tr, up_direction):
#	up_direction.normalized()
#	tr.basis.y.normalized()
#	var cos_normals = tr.basis.y.dot(up_direction)
#	var alpha = acos(cos_normals)
#
#	var axis = tr.basis.y.cross(up_direction)
#	axis = axis.normalized()
#
#	if !is_nan(alpha):
#		tr = tr.rotated(axis, alpha)
#
#	return tr

func new_look_at(target, tr):
	var x = Vector3()
	var y = Vector3()
	var z = Vector3()
	
	y = tr.origin - target
	y.normalized()
	z = tr.basis.y
	x = z.cross(y)
	z = y.cross(x)
	x.normalized()
	z.normalized()
	
	tr.basis.x = x
	tr.basis.y = y
	tr.basis.z = z
	return tr

# Called when the node enters the scene tree for the first time.
func _ready():
	camera = get_node("Camera").get_global_transform()
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
	if Input.is_action_pressed("move_back"):
		direction.z += -1
	if Input.is_action_pressed("move_left"):
		direction.x += 1
	if Input.is_action_pressed("move_right"):
		direction.x += -1
	
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
