<?php
require 'admin-header.php';
session_start();
$id = $_SESSION["admin_email"];
?>
    
    <!-- ***** Search Area ***** -->

    <!-- ***** Main Banner Area Start ***** -->
    <div class="main-banner header-text" id="top">
        <div class="Modern-Slider">
          <!-- Item -->
          <div class="item">
            <div class="img-fill">
                <img src="assets/images/slide-03.jpg" alt="">
                <div class="text-content responsive_view">
                    <?php
                    //echo $id;	
                   if($id=="jubayersohel4@gmail.com") {
                    echo "<a href='list_of_users.php' class='main-stroked-button'>All Users</a>";
                    echo "<a href='manage_user.php' class='main-stroked-button'>Manage User</a>";
                    echo "<a href='add-admin-page.php' class='main-stroked-button'>Add Admin</a>";
                  }
                  
                  ?>
                  <a href="admin-quiz-category.php" class="main-stroked-button">CRUD Quiz</a>
                </div>
            </div>
          </div>
          <!-- // Item -->
        </div>
    </div>
    <!-- ***** Main Banner Area End ***** -->
    
    <?php
    require 'admin-footer.php';
    ?>