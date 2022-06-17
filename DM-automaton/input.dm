/mob/living/simple_animal/hostile/retaliate
	///A list of weakrefs pointing at things that we consider targets
	var/list/enemies = list()

/mob/living/simple_animal/hostile/retaliate/Found(atom/A)
	if(isliving(A))
		var/mob/living/L = A
		if(!L.stat)
			return L
		else
			enemies -= WEAKREF(L)
	else if(ismecha(A))
		var/obj/vehicle/sealed/mecha/M = A
		if(LAZYLEN(M.occupants))
			return A

/mob/living/simple_animal/hostile/retaliate/ListTargets()
	if(!enemies.len)
		return list()
	var/list/see = ..()
	var/list/actual_enemies = list()
	for(var/datum/weakref/enemy as anything in enemies)
		var/mob/flesh_and_blood = enemy.resolve()
		if(!flesh_and_blood)
			enemies -= enemy
			continue
		actual_enemies += flesh_and_blood

	see &= actual_enemies // Remove all entries that aren't in enemies
	return see

/mob/living/simple_animal/hostile/retaliate/proc/Retaliate()
	var/list/around = view(src, vision_range)

	for(var/atom/movable/A in around)
		if(A == src)
			continue
		if(isliving(A))
			var/mob/living/M = A
			if(faction_check_mob(M) && attack_same || !faction_check_mob(M))
				enemies |= WEAKREF(M)
		else if(ismecha(A))
			var/obj/vehicle/sealed/mecha/M = A
			if(LAZYLEN(M.occupants))
				enemies |= WEAKREF(M)
				add_enemies(M.occupants)

	for(var/mob/living/simple_animal/hostile/retaliate/H in around)
		if(faction_check_mob(H) && !attack_same && !H.attack_same)
			H.enemies |= enemies

/mob/living/simple_animal/hostile/retaliate/adjustHealth(amount, updating_health = TRUE, forced = FALSE)
	. = ..()
	if(. > 0 && stat == CONSCIOUS)
		Retaliate()

/mob/living/simple_animal/hostile/retaliate/proc/add_enemies(new_enemies)
	for(var/new_enemy in new_enemies)
		enemies |= WEAKREF(new_enemy)

/mob/living/simple_animal/hostile/retaliate/frog
	name = "frog"
	desc = "They seem a little sad."
	icon_state = "frog"
	icon_living = "frog"
	icon_dead = "frog_dead"
	mob_biotypes = MOB_ORGANIC|MOB_BEAST
	speak = list("ribbit","croak")
	emote_see = list("hops in a circle.", "shakes.")
	speak_chance = 1
	turns_per_move = 5
	maxHealth = 15
	health = 15
	melee_damage_lower = 5
	melee_damage_upper = 5
	attack_verb_continuous = "bites"
	attack_verb_simple = "bite"
	response_help_continuous = "pets"
	response_help_simple = "pet"
	response_disarm_continuous = "pokes"
	response_disarm_simple = "poke"
	response_harm_continuous = "splats"
	response_harm_simple = "splat"
	density = FALSE
	faction = list("hostile")
	attack_sound = 'sound/effects/reee.ogg'
	butcher_results = list(/obj/item/food/nugget = 1)
	pass_flags = PASSTABLE | PASSGRILLE | PASSMOB
	mob_size = MOB_SIZE_TINY
	gold_core_spawnable = FRIENDLY_SPAWN
	can_be_held = TRUE
	held_w_class = WEIGHT_CLASS_TINY
	worn_slot_flags = ITEM_SLOT_HEAD
	head_icon = 'icons/mob/animal_item_head.dmi'
	var/stepped_sound = 'sound/effects/huuu.ogg'
	///How much of a reagent the mob injects on attack
	var/poison_per_bite = 3
	///What reagent the mob injects targets with
	var/poison_type = /datum/reagent/drug/space_drugs

/mob/living/simple_animal/hostile/retaliate/frog/Initialize(mapload)
	. = ..()

	ADD_TRAIT(src, TRAIT_VENTCRAWLER_ALWAYS, INNATE_TRAIT)

	if(prob(1))
		name = "rare frog"
		desc = "They seem a little smug."
		icon_state = "rare_frog"
		icon_living = "rare_frog"
		icon_dead = "rare_frog_dead"
		butcher_results = list(/obj/item/food/nugget = 5)
		poison_type = /datum/reagent/drug/mushroomhallucinogen

	var/static/list/loc_connections = list(
		COMSIG_ATOM_ENTERED = .proc/on_entered,
	)
	AddElement(/datum/element/connect_loc, loc_connections)
	AddElement(/datum/element/venomous, poison_type, poison_per_bite)
	add_cell_sample()

/mob/living/simple_animal/hostile/retaliate/frog/proc/on_entered(datum/source, AM as mob|obj)
	SIGNAL_HANDLER
	if(!stat && isliving(AM))
		var/mob/living/L = AM
		if(L.mob_size > MOB_SIZE_TINY)
			playsound(src, stepped_sound, 50, TRUE)

/mob/living/simple_animal/hostile/retaliate/frog/add_cell_sample()
	. = ..()
	AddElement(/datum/element/swabable, CELL_LINE_TABLE_FROG, CELL_VIRUS_TABLE_GENERIC_MOB, 1, 5)

/mob/living/simple_animal/hostile/retaliate/frog/rare
	name = "rare frog"
	desc = "They seem a little smug."
	icon_state = "rare_frog"
	icon_living = "rare_frog"
	icon_dead = "rare_frog_dead"
	butcher_results = list(/obj/item/food/nugget = 5)
	poison_type = /datum/reagent/drug/mushroomhallucinogen

/mob/living/simple_animal/hostile/retaliate/frog/rare/on_entered(datum/source, AM as mob|obj)
	//example override

/mob/living/simple_animal/hostile/retaliate/frog/rare/on_entered(datum/source)
	//example parameter overloading