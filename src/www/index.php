<?php
    $title = 'Sources | Kinect Over Web';

    $WEB_ROOT;
    $SITE_ROOT;
    $DOCUMENT_ROOT = $_SERVER["DOCUMENT_ROOT"];
    require_once "$DOCUMENT_ROOT/roots.php";
    require_once "$SITE_ROOT/assets/php/main.php";
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <?php echo execAndRead("{$SITE_ROOT}/assets/php/head.php"); ?>
    <link rel="stylesheet" href="./index.css">
    <script src="./index.js" type="module"></script>
</head>
<header id="header"><?php echo execAndRead("{$SITE_ROOT}/assets/php/header.php"); ?></header>
<body>
    <div>
        <section id="splashTitleContainer">
            <div class="center">
                <h1>Kinect Over Web</h1>
                <h4>View the sources that your kinect is outputting</h4>
            </div>
        </section>
        <section>
            <h3>Sources</h3>
            <hr>
            <br>
            <table id="sourcesList">
                <tbody>
                    <tr>
                        <td>
                            <div class="bottomStrip">
                                <a href="<?php echo $WEB_ROOT ?>/view-source/Body_Points_Mapped_To_Colour/">
                                    <h4>Body points mapped to colour</h4>
                                    <p>
                                        Draws the skeletons of the found bodies with their positions mapped to the colour camera.<br>
                                        Good for overlaying on top of the colour camera.
                                    </p>
                                </a>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </section>
    </div>
</body>
<footer id="footer"><?php echo execAndRead("{$SITE_ROOT}/assets/php/footer.php"); ?></footer>
</html>