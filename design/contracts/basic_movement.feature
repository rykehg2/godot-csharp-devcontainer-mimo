Feature: Basic Platformer Movement
    As a Player
    I want to move and jump
    So that I can navigate the environment

    Background:
        Given a player character "Player" exists
        And the player is a "CharacterBody2D"
        And a ground "Ground" exists as a "StaticBody2D"

    Scenario: Gravity applies force when in mid-air
        Given the player is at position "Y=0"
        And there is no floor at "Y=0"
        When 1.0 seconds of physics processing pass
        Then the player's "Y" position should be greater than 0

    Scenario: Horizontal movement reaches target speed
        Given the player is on the floor
        When the "ui_right" action is pressed
        Then the player's velocity "X" should reach "400.0" units/sec

    Scenario: Jumping applies vertical impulse
        Given the player is on the floor
        When the "ui_select" action is triggered
        Then the player's velocity "Y" should be exactly "-600.0"