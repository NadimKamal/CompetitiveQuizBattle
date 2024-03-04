
<?php
    $cat = $_GET['category'];
    $main_sub = $cat;
    if($cat =="school")
    {
        $class = $_GET['class'];
        $sub = $_GET['subject'];
        $main_sub .= ",".$class.",".$sub;
    }
    else if($cat =="college")
    {
        $main_sub .= ",".$_GET['subject'];
    }
    else if($cat == "university")
    {
        $main_sub .= ",".$_GET['dept'].",".$_GET['course'];
    }
    //echo $main_sub;
    ob_start();
    require 'user-header.php';
    //require 'connection.php';
    if($_POST["challenge"])
    {
        //echo $_GET['category'].'2';
        $opponent = $_POST["search-user"];
        //$category = $_GET["category"];
        if($opponent!="")
        {
            //echo $opponent;
            $get_data = "Select * from UserTable where email='$opponent'";
            $qry = $conn->prepare($get_data);
            $qry->execute();
            //echo $get_data;
            $cnt = $qry->rowCount();
            $result = $qry->fetchAll();
            $opponent_id = $result[0][0];
            //echo $opponent_id;
            if($cnt!=0)
            {


                //Random question selector start

                $question = "";

                //echo $cat[0];
                
                if($cat[0]=='s')
                {
                    $category = "school";
                    $first = 0;
                    $second = 0;
                    $class="";
                    $subject="";
                    //echo strlen($cat);
                    for($i = 0;$i<strlen($cat); $i++)
                    {
                        if($first == 0 && $second == 0)
                        {
                            if($cat[$i]==',')
                            {
                                $first = 1;
                            }
                        }
                        else if($first == 1 && $second == 0)
                        {
                            
                            if($cat[$i]==',')
                            {
                                $second = 1;
                            }
                            else 
                            {
                                $class.=$cat[$i];
                            }
                        }
                        else if($first == 1 && $second == 1)
                        {
                            $subject.=$cat[$i];
                        }
                    }

                    //echo $dept;
                   // echo $course;
                    
                    $school_question = "SELECT * from school where class_id=$class && subject_id=$subject";
                    $qry = $conn->prepare($school_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }
                    
                }
                if($cat[0]=='u')
                {
                    $category = "university";
                    $first = 0;
                    $second = 0;
                    $dept="";
                    $course="";
                    //echo strlen($cat);
                    for($i = 0;$i<strlen($cat); $i++)
                    {
                        if($first == 0 && $second == 0)
                        {
                            if($cat[$i]==',')
                            {
                                $first = 1;
                            }
                        }
                        else if($first == 1 && $second == 0)
                        {
                            
                            if($cat[$i]==',')
                            {
                                $second = 1;
                            }
                            else 
                            {
                                $dept.=$cat[$i];
                            }
                        }
                        else if($first == 1 && $second == 1)
                        {
                            $course.=$cat[$i];
                        }
                    }

                    //echo $dept;
                   // echo $course;
                    
                    $university_question = "SELECT * from university where dept_id=$dept && course_id=$course";
                    $qry = $conn->prepare($university_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }

                }

                if($cat[0]=='c')
                {
                    $category = "college";
                    $first = 0;
                    $subject="";
                    //echo strlen($cat);
                    for($i = 0;$i<strlen($cat); $i++)
                    {
                        if($first == 0)
                        {
                            if($cat[$i]==',')
                            {
                                $first = 1;
                            }
                        }
                        else if($first == 1)
                        {
                            $subject.=$cat[$i];
                        }
                    }

                    //echo $dept;
                   // echo $course;
                    
                    $college_question = "SELECT * from college where subject_id=$subject";
                    $qry = $conn->prepare($college_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }

                }
                if($cat[0]=='g')
                {
                    $category = "gk";
                    //echo $dept;
                   // echo $course;
                    
                    $gk_question = "SELECT * from gk";
                    $qry = $conn->prepare($college_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }

                }
                

                //Random question selector end


                $cat = $_GET['category'];
                //echo $cnt;
                session_start();
                $user_id = $_SESSION["user_id"];
                $my_name = $_SESSION["user_name"];
                //echo $cat;
                //echo $my_name;
                //echo $my_email;
                $notification_details = "New Challenge from ". $my_name. " for Category ".$cat;
                $details =$my_name. " Challenged You";
                //echo $notification_details;
                $insert_notification = "INSERT into Notifications (user_id,opponent_id,notification,details,category,flag,question) values($user_id,$opponent_id,'$notification_details','$details','$cat',0,'$question')";
                $conn->exec($insert_notification);
                //echo "<script>alert('Geche')</script>";
                //$var = 'https://www.google.com/';
                //echo "<script>alert('sesh Geche')</script>";
                header('Location: waiting-page.php');
                exit;
            }
                
        }
    }
    
    
    
    if($_POST["practice"])
    {
        //echo $_GET['category'].'2';
        //$opponent = $_POST["search-user"];
        //$category = $_GET["category"];
        
            //echo $opponent;
            


                //Random question selector start

                $question = "";

                //echo $cat[0];
                
                if($cat[0]=='s')
                {
                    $category = "school";
                    $first = 0;
                    $second = 0;
                    $class="";
                    $subject="";
                    //echo strlen($cat);
                    for($i = 0;$i<strlen($cat); $i++)
                    {
                        if($first == 0 && $second == 0)
                        {
                            if($cat[$i]==',')
                            {
                                $first = 1;
                            }
                        }
                        else if($first == 1 && $second == 0)
                        {
                            
                            if($cat[$i]==',')
                            {
                                $second = 1;
                            }
                            else 
                            {
                                $class.=$cat[$i];
                            }
                        }
                        else if($first == 1 && $second == 1)
                        {
                            $subject.=$cat[$i];
                        }
                    }

                    //echo $dept;
                   // echo $course;
                    
                    $school_question = "SELECT * from school where class_id=$class && subject_id=$subject";
                    $qry = $conn->prepare($school_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }
                    
                }
                if($cat[0]=='u')
                {
                    $category = "university";
                    $first = 0;
                    $second = 0;
                    $dept="";
                    $course="";
                    //echo strlen($cat);
                    for($i = 0;$i<strlen($cat); $i++)
                    {
                        if($first == 0 && $second == 0)
                        {
                            if($cat[$i]==',')
                            {
                                $first = 1;
                            }
                        }
                        else if($first == 1 && $second == 0)
                        {
                            
                            if($cat[$i]==',')
                            {
                                $second = 1;
                            }
                            else 
                            {
                                $dept.=$cat[$i];
                            }
                        }
                        else if($first == 1 && $second == 1)
                        {
                            $course.=$cat[$i];
                        }
                    }

                    //echo $dept;
                   // echo $course;
                    
                    $university_question = "SELECT * from university where dept_id=$dept && course_id=$course";
                    $qry = $conn->prepare($university_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }

                }

                if($cat[0]=='c')
                {
                    $category = "college";
                    $first = 0;
                    $subject="";
                    //echo strlen($cat);
                    for($i = 0;$i<strlen($cat); $i++)
                    {
                        if($first == 0)
                        {
                            if($cat[$i]==',')
                            {
                                $first = 1;
                            }
                        }
                        else if($first == 1)
                        {
                            $subject.=$cat[$i];
                        }
                    }

                    //echo $dept;
                   // echo $course;
                    
                    $college_question = "SELECT * from college where subject_id=$subject";
                    $qry = $conn->prepare($college_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }

                }
                if($cat[0]=='g')
                {
                    $category = "gk";
                    //echo $dept;
                   // echo $course;
                    
                    $gk_question = "SELECT * from gk";
                    $qry = $conn->prepare($college_question);
                    $qry->execute();
                    //echo $get_data;
                    $cnt = $qry->rowCount();
                    $result = $qry->fetchAll();

                    $map_array = array();
                    $size = $result[$cnt-1][0];
                    for($i=0;$i<=$size;$i++)
                    {
                        array_push($map_array,0);
                    }
                    
                    if($cnt!=0)
                    {
                        //echo $cnt;
                        $arr = array();
                        for($i = 0; $i<$cnt; $i++)
                        {
                            array_push($arr,$result[$i][0]);
                        }

                        //print_r ($arr);
                        //$finally = 0;
                        //$set = new \Ds\Set();
                        //$final_array = array();
                        //$result_array = array();
                        $counter_value = 0;
                        //echo sizeof(array_unique($final_array));
                        while(1)
                        {
                            if($counter_value==5)
                            {
                                break;
                            }
                            else{
                                $indx = rand(0,$cnt-1);
                                if($map_array[$arr[$indx]]==0)
                                {
                                    $map_array[$arr[$indx]]=1;
                                    $counter_value++;
                                    $question.=$arr[$indx];
                                    if($counter_value<5)
                                    {
                                        $question.=',';
                                    }
                                }
                            }
                        }
                    }

                }
                

                //Random question selector end


                $cat = $_GET['category'];
                //echo $cnt;
                session_start();
                $_SESSION["question"]=$question;
                $_SESSION["cat"]=$cat;
                
                header('Location: practice_page.php');
                exit;
            }
                
    
    
    
    
?>
    <!-- ***** Main Banner Area Start ***** -->
    <div class="main-banner header-text" id="top">
        <div class="Modern-Slider">
          <!-- Item -->
          <div class="item">
            <div class="img-fill">
                <img src="assets/images/slide-03.jpg" alt="">
                <div class="text-content">
                <form class="form form-login" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"])."?category=".$main_sub;?>">
                        <div class="form-group">
                            <input name="search-user" type="text" placeholder="Opponent's Email" required>
                        </div>
                        <div class="col-lg-14">
                            <fieldset>
                            <input type="submit" class="btn-challenge" name="challenge" value="Challenge"/>
                            </fieldset>
                        </div>
                        
                        </form>
                        <form class="form form-login" method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"])."?category=".$main_sub;?>">
                        <div class="col-lg-14">
                            <fieldset>
                            <input type="submit" class="btn-challenge" name="practice" value="Practice"/>
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
        require 'user-footer.php';
    ?>