//lets try to create some false positives
/not_a_base_type/thing/name
/not_a_base_type/thing/name(/var/parameter)
/datum/thing(/var/parameter)another_thing/name

//now for some positives
/mob/sentient/animal
/mob/sentient/animal/proc/create()

/mob/sentient/animal/dog
/mob/sentient/animal/dog/create()
/mob/sentient/animal/dog/proc/bark()

/mob/sentient/animal/cat
/mob/sentient/animal/cat/create()
/mob/sentient/animal/cat/proc/meow()

/mob/sentient/animal/dog/corgi
/mob/sentient/animal/dog/corgi/create()