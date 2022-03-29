<?php
$password = $_POST['password'];
try {
	//Check if the matching username and password
	$query = $db->prepare('SELECT password FROM gameDB WHERE username = ?');
	$query->execute(array($_POST['username']));

	if ($query->rowCount() == 1) {// If there is an account with matching username.
		$row = $query->fetch();

		if (password_verify($_POST['password'], $row['password'])) { //Passwords match

			return true;
        }
    }
    return false;
}

<?php