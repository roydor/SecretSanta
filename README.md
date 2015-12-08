# SecretSanta
Secret Santa Mailer is a tool used by WestCoastFamily to draw names and email their result to everyone.
It makes for a simple way, that everyone can participate, while flexing the nerd muscles of the group.

# Algorithm
The tool constructs a graph where the vertices of the graph represent the participants in the exchange, and the edges represent the ability to gift another person.  Then, the participant choosing their recipient is always the one with the fewest restrictions possible, and they will chose from their valid recipients at random.
I didn't see any problems with that approach in over 20,000 test runs, so without a proof it seems solid.

Even if there were to be a problem, there is retry logic in place to ensure it works the first time it is ran.

# Mailer
I had >>a lot<< of trouble getting the gmail account to work, to be mailed through this tool.

I needed to:
- Enable 'Less Secure' apps to use the account.
- Set up 2-step verification, create and use an app specific password.


