<?php
    require 'user-header.php';
 
?>
    <!-- ***** Main Banner Area Start ***** -->
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
          box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
          z-index: 1;
        }
        
        .dropdown-content a {
          color: black;
          padding: 12px 16px;
          text-decoration: none;
          display: block;
        }
        
        .dropdown-content a:hover {background-color: #ddd;}
        
        .dropdown:hover .dropdown-content {display: block;}
        
        .dropdown:hover .dropbtn {background-color: #3e8e41;}
        </style>
    <div class="main-banner header-text" id="top">
        <div class="Modern-Slider">
            <!-- Item -->
            <div class="item">
                <div class="img-fill">
                    <img src="assets/images/slide-03.jpg" alt="">
                    <div class="text-content">
                        <div class="dropdown">
                            <button class="dropbtn">Select Class</button>
                            <div class="dropdown-content">
                            <?php
                                try{
                                    $get_data = "Select * from schoolClass";
                                    $qry = $conn->prepare($get_data);
    		                        $qry->execute();
    		                        //echo $get_data;
    		                        $cnt = $qry->rowCount();
    		                        $result = $qry->fetchAll();
                                    //echo $cnt;
                                    for($i = 0; $i<$cnt; $i++)
                                    {
                                        echo "<a href='school-class.php?category=".$_GET['category']."&class=".$result[$i][0]."'>".$result[$i][1]."</a>";
                                    }
                                }
                                catch(Exception $e)
                                {
                                    echo $e;
                                }
                                
                                ?>
                            </div>
                        </div>
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