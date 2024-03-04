
<?php
require 'admin-header.php';
require 'connection.php';

    //echo "geche";
    //echo "Print";
    
    $subject_name = $_POST["add-subject"];
    $class = $_GET['class'];
    if($subject_name!="")
    {
        $sql = "INSERT into School_Subject (subject_name,class_id) values('$subject_name',$class)";
        $conn->exec($sql);
        $var = 'http://'.$_SERVER['HTTP_HOST'].$_SERVER['REQUEST_URI'];
        header("Location: $var");
    }
?>

    <style>
        .dropbtn {
            background-color: #04AA6D;
            color: white;
            padding: 16px;
            font-size: 16px;
            border: none;
        }

        .dropdown {
            position: relative;
            display: inline-block;
        }

        .dropdown-content {
            display: none;
            position: absolute;
            background-color: #04AA6D;
            min-width: 160px;
            box-shadow: 0px 8px 16px 0px rgba(0, 0, 0, 0.2);
            z-index: 1;
        }

        .dropdown-content a {
            color: black;
            padding: 12px 16px;
            text-decoration: none;
            display: block;
        }

        .dropdown-content a:hover {
            background-color: #ddd;
        }

        .dropdown:hover .dropdown-content {
            display: block;
        }

        .dropdown:hover .dropbtn {
            background-color: #3e8e41;
        }
    </style>


   

    <!-- ***** Main Banner Area Start ***** -->
    <div class="main-banner header-text" id="top">
        <div class="Modern-Slider">
            <!-- Item -->
            <div class="item">
                <div class="img-fill">
                    <img src="assets/images/slide-03.jpg" alt="">
                    <div class="text-content">
                        <div class="dropdown">
                            <button class="dropbtn">Select Subject</button>
                            <div class="dropdown-content">
                            <?php
                                $get_data = "Select * from School_Subject where class_id=$class";
                                $qry = $conn->prepare($get_data);
		                        $qry->execute();
		                        //echo $get_data;
		                        $cnt = $qry->rowCount();
		                        $result = $qry->fetchAll();
                                //echo $cnt;
                                for($i = 0; $i<$cnt; $i++)
                                {
                                    echo "<a href='add-quiz-form.php?class=".$_GET['class']."&subject=".$result[$i][0]."'>".$result[$i][1]."</a>";
                                }
                                
                                ?>
                            </div>
                        </div>
                        <br>
                        <br>
                        <form method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"])."?class=".$_GET['class'];?>">
                        <div class="form-group">
                            <input name="add-subject" type="text" required>
                        </div>
                        <div class="col-lg-14">
                            <fieldset>
                              <button name="add-subject-button" type="submit" id="form-submit" class="main-button-icon">Add Subject<i class="fa fa-arrow-right"></i></button>
                            </fieldset>
                        </div>
                        </form>
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