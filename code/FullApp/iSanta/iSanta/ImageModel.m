//
//  ImageModel.m
//  iSanta
//
//  Created by Siegle, Andrew L on 9/11/12.
//
//

#import "ImageModel.h"

@implementation ImageModel


@synthesize circleImage = _circleImage;
@synthesize normalBlackImage = _normalBlackImage;
@synthesize normalPinkImage = _normalPinkImage;
@synthesize normalOrangeImage = _normalOrangeImage;
@synthesize editImage = _editImage;
@synthesize animationArray = _animationArray;

@synthesize upImage = _upImage;
@synthesize dnImage = _dnImage;
@synthesize lfImage = _lfImage;
@synthesize rtImage = _rtImage;

NSString *const BLACKPATH       =   @"normal-black";
NSString *const ORANGEPATH      =   @"normal-orange";
NSString *const PINKPATH        =   @"normal-pink";
NSString *const CIRCLEPATH      =   @"circle-crosshair";
NSString *const EDITPATH        =   @"edit-circle-crosshair";
NSString *const OPENGREENPATH   =   @"circle.png";
NSString *const OPENREDPATH     =   @"redcircle.png";
NSString *const UPIMAGE         =   @"btn_up";
NSString *const DOWNIMAGE       =   @"btn_dn";
NSString *const LEFTIMAGE       =   @"btn_lf";
NSString *const RIGHTIMAGE      =   @"btn_rt";


- (UIImage *)normalBlackImage
{
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:BLACKPATH
                                         ofType:@"png"];
    if (!_normalBlackImage) _normalBlackImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _normalBlackImage;
}

- (UIImage *)normalOrangeImage
{
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:ORANGEPATH
                                         ofType:@"png"];
    if (!_normalOrangeImage) _normalOrangeImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _normalOrangeImage;
}
- (UIImage *)normalPinkImage
{
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:PINKPATH
                                         ofType:@"png"];
    if (!_normalPinkImage) _normalPinkImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _normalPinkImage;
}

- (UIImage *)circleImage
{
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:CIRCLEPATH
                                         ofType:@"png"];
    if (!_circleImage) _circleImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _circleImage;
}

- (UIImage *)editImage
{
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:EDITPATH
                                         ofType:@"png"];
    if (!_editImage) _editImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _editImage;
}


- (NSArray *)animationArray
{
    if (!_animationArray) _animationArray = [[NSArray alloc] initWithObjects:[UIImage imageNamed:OPENGREENPATH],
                                             [UIImage imageNamed:OPENREDPATH], nil];
    return _animationArray;
}


- (UIImage *) upImage{
    if (!_upImage) _upImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:UPIMAGE ofType:@"png"]];
    return _upImage;
}
- (UIImage *) dnImage{
    if (!_dnImage) _dnImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:DOWNIMAGE ofType:@"png"]];
    return _dnImage;
}
- (UIImage *) lfImage{
    if (!_lfImage) _lfImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:LEFTIMAGE ofType:@"png"]];
    return _lfImage;
}
- (UIImage *) rtImage{
    if (!_rtImage) _rtImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:RIGHTIMAGE ofType:@"png"]];
    return _rtImage;
}



@end
