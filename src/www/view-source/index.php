<?php
    $WEB_ROOT;
    $SITE_ROOT;
    $DOCUMENT_ROOT = $_SERVER["DOCUMENT_ROOT"];
    require_once "$DOCUMENT_ROOT/roots.php";
    require_once "$SITE_ROOT/assets/php/main.php";
    global $REQUEST_URI;

    if ($REQUEST_URI[strlen($REQUEST_URI) - 1] != '/')
    {
        header('Location: ' . $REQUEST_URI . '/', true, 301);
        die();
    }

    $sources = array(
        '/Body_Points_Mapped_To_Colour/'
    );

    $path = str_replace($WEB_ROOT . '/view-source', '', $REQUEST_URI);
    if (!in_array($path, $sources))
    {
        http_response_code(404);
        die();
    }
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <?php echo execAndRead("{$SITE_ROOT}/assets/php/head.php"); ?>
    <link rel="stylesheet" href="<?php echo $WEB_ROOT; ?>/view-source/view-source.css">
    <script src="<?php echo $WEB_ROOT; ?>/view-source/view-source.js" type="module"></script>
</head>
<header id="header"><?php echo execAndRead("{$SITE_ROOT}/assets/php/header.php"); ?></header>
<body>
    <span class="slideMenu"></span>
    <canvas id="canvas"></canvas>
</body>
<footer id="footer"></footer>
</html>