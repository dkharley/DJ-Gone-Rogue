//This is the Readme file for DJ_Gone_Rogue

To start a local repo:

	cd MyDirectory
	git init
	git remote add origin https://github.com/cptcaveman/DJ_Gone_Rogue.git
	git pull origin master

git init 
	makes an empty local repository in your directory
git remote add origin ... 
	sets the origin of the  remote repo
	remote signifies the repo is not local.
	add tells git to add the repo
	origin tells git that the  url is the origin of the remote repo
git pull origin master 
	pulls data from the repo represented by origin
	in this case, it is the  github repo and then syncs the data with
	your local master branch

after these commands, you should have the project in the directory you
specified. you can test this by trying to open the projectin Unity.

Now we will cover branching

to check which local branches you have currently, enter
	git branch
the prompt should spit out iinfo about active branches

git branch as of now should show "* master", in green (signifies 
your current branch), as the only branch

To make a branch, enter
	git branch name_of_branch
	git checkout name_of_branch
		or
	git checkout -b name_of_branch
You  should now be in the branch you just made
To verify, enter
	git branch
You should now have two branches with the one you just created in green
To switch branches, enter
	git checkout name_of_branch
Ex:
	git checkout master    will take you to the master branch
Switch to the branch you just made. 

We will now cover merging two branches and committing work.
This is important for once you complete tasks and want to commit your work.

Now that you are in your new branch, make a new text file
	touch test.txt
If you enter ls, you should now see that you have test.txt in your directory

To commit the new  file you just made
	git add test.txt
This adds the file to a list of files that you will later commit
To actually make the  commit
	git commit -m "Adding test file"
The -m tells git that the next param in the bash  command is going to be
a message. This  message should tell us what changes are made by this
commit.

now that we have told git that we want to commit this test file, and we
have made sure the new additions do what we expect and it doesnt break the
build, we can merge our branches
We want to merge then new branch with our master branch
So,
	git checkout master
	git merge name_of_branch
Git should spit out that these branches were merged
If you want to delete the  otherbranch
	git branch -d name_of_branch
never delete the  master branch and only delete a branch if you are
sure you won't need it later.

Now to push your changes to the repo,
	git push

Doneskies

Here's all the  command entries

cd MyDirectory
git init
git remote add origin https://github.com/cptcaveman/DJ_Gone_Rogue
git pull origin master
git branch
git checkout -b test_branch
git branch
touch test.txt
git add test.txt
git commit -m "Adding test file"
git status
git checkout master
git push

Don't actually push cuz I don't want that shit on the  repo. but you get 
the idea. Always make changes on branches. Don't make actually code changes 
in your master branch because you don't want to fuck that shit up. If you 
are gonna make changes, work on a branch and then merge that branch with 
your master branch then push to the remote repository
>>>>>>> 6e9dc7075d0db559f83938792f6eec876e83b279
