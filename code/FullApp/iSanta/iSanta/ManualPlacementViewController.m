//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//


#import "ManualPlacementViewController.h"
#import "DetailViewController.h"
#import <QuartzCore/QuartzCore.h>
//#import "ImageRecController.h"



@interface ManualPlacementViewController()
@property (nonatomic, strong) NSMutableArray *ivArray;
@property (nonatomic, strong) UIActionSheet *modeSheet;
@property (nonatomic, strong) UIButton *upButton;
@property (nonatomic, strong) UIButton *dnButton;
@property (nonatomic, strong) UIButton *lfButton;
@property (nonatomic, strong) UIButton *rtButton;

@property (nonatomic, strong) UIImageView *selectedPoint;
- (void)buttonsVisible:(BOOL) vis;
-(void) centerAtX:(int)x andY:(int)y;
- (void) switchModeTo:(int) mode;
@end



@implementation ManualPlacementViewController
@synthesize brain = _brain;
@synthesize tapRecognizer = _tapRecognizer;
@synthesize longPressRec = _longPressRec;
@synthesize singleTapRec = _singleTapRec;
@synthesize scrollView = _scrollView;
@synthesize masterView = _masterView;
@synthesize imageView = _imageView;
@synthesize ivArray = _ivArray;
@synthesize modeSheet = _modeSheet;
@synthesize detailView = _detailView;
@synthesize upButton = _upButton;
@synthesize dnButton = _dnButton;
@synthesize lfButton = _lfButton;
@synthesize rtButton = _rtButton;

@synthesize selectedPoint = _selectedPoint;
@synthesize images = _images;

NSString *const THREEPOINTSPROMPT= @"Add three points of a right angle on target";
NSString *const MOVEPOINTSPROMPT = @"Tap to select a point to move";
NSString *const DELETEPOINTSPROMPT = @"Tap a point to delete";

NSString *const NORMALMESSAGE = @"NOTE: The first three points (pink, yellow, black) should mark a right angle on the target of known dimensions. Pink should be above Orange, and Black to the right of Pink.  This is used for image normalization\n\nSingle tap: Add point\nLong tap: Mode Select (Delete / Move)";
NSString *const MOVINGMESSAGE = @"Single tap: Select point\nDouble tap: Return to normal view\nLong tap: Mode select (Delete / Add)";
NSString *const DELETEMESSAGE = @"Single tap: Delete point\nDouble tap: Return to normal view\nLong tap: Mode select (Move / Add)";



//bool deleting;
int status;
const int NORMAL = 0;
const int DELETING = 1;
const int MOVING = 2;

const double ALPHAVAL = .45;
const double ANIMATEDURATION = 1;
const int DISTTHRESHOLD = 30;
const int EDITTHRESHOLD = 20;

const int BTNSMALL = 60;
const int BTNBIG = 125;
const double BTNALPHA = .5;

int selectedPointIndex = -1;

double XCenterOffset,YCenterOffset;


int currentOp = 1;

- (void) donePlacing{
    NSString *message;
    switch (status) {
        case NORMAL:
            message = NORMALMESSAGE;
            break;
            
        case MOVING:
            message = MOVINGMESSAGE;
            break;
            
        case DELETING:
            message = DELETEMESSAGE;
            
        default:
            break;
    }
    
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Manual Impact Placement" message:message delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
    
    [alert show];
}

- (void)viewWillDisappear:(BOOL)animated
{
    UIImage *image;
    CGPoint pt = self.masterView.bounds.origin;
    
    UIGraphicsBeginImageContext(self.masterView.bounds.size);
    CGContextRef context = UIGraphicsGetCurrentContext();
    CGContextConcatCTM(context, CGAffineTransformMakeTranslation(-(int)pt.x, -(int)pt.y));
    [self.masterView.layer renderInContext:context];
    image = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    NSData *data = UIImageJPEGRepresentation(image, 0.85);
    
    [self.detailView saveExportImage:data];
}

- (void) viewDidLoad
{
    self.imageView.image = self.brain.targetImage;
    self.view.backgroundColor = [UIColor blackColor];
    [self.masterView addGestureRecognizer:[self tapRecognizer]];
    [self.masterView addGestureRecognizer:[self longPressRec]];
    [self.masterView addGestureRecognizer:[self singleTapRec]];
    
    
    self.scrollView.minimumZoomScale=1.0;
    self.scrollView.maximumZoomScale = 3.0;
    self.scrollView.delegate = self;
    
    self.masterView.backgroundColor = [UIColor blackColor];
    

    [self.scrollView addSubview:self.masterView];
    [self.masterView addSubview:self.imageView];
    self.navigationItem.rightBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Help" style:UIBarButtonItemStylePlain target:self action:@selector(donePlacing)];

    status = NORMAL;
    
    [self.view addSubview:self.upButton];
    [self.view addSubview:self.dnButton];
    [self.view addSubview:self.lfButton];
    [self.view addSubview:self.rtButton];
    
    [self.upButton setImage:self.images.upImage forState:UIControlStateNormal];
    [self.dnButton setImage:self.images.dnImage forState:UIControlStateNormal];
    [self.lfButton setImage:self.images.lfImage forState:UIControlStateNormal];
    [self.rtButton setImage:self.images.rtImage forState:UIControlStateNormal];
    
    self.upButton.hidden = YES;
    self.dnButton.hidden = YES;
    self.lfButton.hidden = YES;
    self.rtButton.hidden = YES;
    
    [self.upButton addTarget:self action:@selector(movePoint:) forControlEvents:UIControlEventTouchUpInside];
    [self.dnButton addTarget:self action:@selector(movePoint:) forControlEvents:UIControlEventTouchUpInside];
    [self.lfButton addTarget:self action:@selector(movePoint:) forControlEvents:UIControlEventTouchUpInside];
    [self.rtButton addTarget:self action:@selector(movePoint:) forControlEvents:UIControlEventTouchUpInside];

    
    [self loadPointsFromBrain];
    [self switchModeTo:NORMAL];
    
    
   
    
}


- (UIButton *) upButton{
    if (!_upButton) {
        _upButton = [[UIButton alloc] initWithFrame:CGRectMake(self.view.center.x - (BTNBIG / 2), 0.0, BTNBIG,BTNSMALL)];
        self.upButton.backgroundColor = [UIColor grayColor];
        _upButton.alpha = BTNALPHA;
        
    }
    return _upButton;
}
- (UIButton *) dnButton{
    if (!_dnButton){
        _dnButton = [[UIButton alloc] initWithFrame:CGRectMake(self.view.center.x - (BTNBIG / 2), self.view.bounds.size.height - BTNSMALL - 27, BTNBIG,BTNSMALL)];
        self.dnButton.backgroundColor = [UIColor grayColor];
        _dnButton.alpha = BTNALPHA;
         
    }
    return _dnButton;
}
- (UIButton *) lfButton{
    if (!_lfButton) {
        _lfButton = [[UIButton alloc] initWithFrame:CGRectMake(0, self.view.center.y - (BTNBIG/ 2), BTNSMALL,BTNBIG)];
        self.lfButton.backgroundColor = [UIColor grayColor];
        _lfButton.alpha = BTNALPHA;
            }
    return _lfButton;
}
- (UIButton *) rtButton{
    if (!_rtButton) {
        _rtButton = [[UIButton alloc] initWithFrame:CGRectMake(self.view.bounds.size.width - BTNSMALL, self.view.center.y - (BTNBIG/2), BTNSMALL,BTNBIG)];
        self.rtButton.backgroundColor = [UIColor grayColor];
        _rtButton.alpha = BTNALPHA;
           }
    return _rtButton;
}

-(void) resetPointImage:(UIImageView *) view{
    [view setImage:self.images.circleImage];
}

-(void) switchModeTo:(int)mode{
    status = mode;
    switch (mode) {
        case NORMAL:
            if (self.selectedPoint != nil)
                [self resetPointImage:self.selectedPoint];
            self.selectedPoint = nil;
            
            //undim
            self.imageView.alpha = 1.0;
            
            //stop animating
            for (UIImageView *iv in self.ivArray) {
                [iv stopAnimating];
            }
            
            [self buttonsHidden:YES];
            self.navigationItem.prompt = THREEPOINTSPROMPT;

            break;
        
        case MOVING:
            if (self.selectedPoint != nil)
                [self resetPointImage:self.selectedPoint];
            self.selectedPoint = nil;
            
            //undim
            self.imageView.alpha = 1.0;
            
            //stop animating
            for (UIImageView *iv in self.ivArray) {
                [iv stopAnimating];
            }
            
            [self buttonsHidden:YES];
            self.navigationItem.prompt = MOVEPOINTSPROMPT;


            break;
            
        case DELETING:
            if (self.selectedPoint != nil)
                [self resetPointImage:self.selectedPoint];
            self.selectedPoint = nil;
            
            //undim
            self.imageView.alpha = ALPHAVAL;
            
            //stop animating
            for (UIImageView *iv in self.ivArray) {
                
                [iv startAnimating];
            }
            
            [self buttonsHidden:YES];
            self.navigationItem.prompt = DELETEPOINTSPROMPT;


            break;
            
        default:
            break;
    }
}


- (void) movePoint:(id) sender{
    if (self.selectedPoint != nil){
        const int MOVEMENT = 2;
        CGPoint loc = self.selectedPoint.center;
        
        if (sender == self.upButton){
            loc.y -= MOVEMENT;
        }else if (sender == self.dnButton) {
            loc.y += MOVEMENT;
        }else if (sender == self.lfButton){
            loc.x -= MOVEMENT;
        }else { //rtButton
            loc.x += MOVEMENT;
        }
        
        //re-center the screen
        [self centerAtX:loc.x andY:loc.y withZoomFactor:self.scrollView.zoomScale];

        //change it on screen
        self.selectedPoint.center = loc;
        
        //change it in brain
        [self.brain replacePointAtIndex:selectedPointIndex withPoint:loc];
        [self.detailView setPoints:self.brain.points];
    }
    
}
                                              

- (UIView *)viewForZoomingInScrollView:(UIScrollView *)scrollView{
    //return self.imageView;
        return self.masterView;
}

-(void) drawAtPoint:(CGPoint) loc{
    UIImageView *iv;
    NSLog(@"%d",self.ivArray.count);
    if (self.ivArray.count == 0){
        iv = [[UIImageView alloc] initWithImage:self.images.normalPinkImage];
        iv.animationImages = nil;


    }else if (self.ivArray.count == 1){
        iv = [[UIImageView alloc] initWithImage:self.images.normalOrangeImage];
        iv.animationImages = nil;


    }else if (self.ivArray.count == 2){
        iv = [[UIImageView alloc] initWithImage:self.images.normalBlackImage];
        iv.animationImages = nil;

    }
    else{
        iv = [[UIImageView alloc] initWithImage:self.images.circleImage];
        iv.animationImages = self.images.animationArray;
    }
//        iv = [[UIImageView alloc] initWithImage:self.brain.circleImage];

    
    iv.center = loc;
    iv.animationDuration = ANIMATEDURATION;
    //        [self.imageView addSubview:iv];
    [self.masterView addSubview:iv];
    //add view to array
    [self.ivArray addObject:iv];
}

- (void) loadPointsFromBrain{
//    for (NSValue *value in self.brain.points){
//        [self drawAtPoint:[value CGPointValue]];
//    }
    for (NSValue *value in self.brain.points){
        CGPoint point = [value CGPointValue];
        point.x += self.imageView.center.x;
        point.y += self.imageView.center.y;
        point.y *= -1;
        [self drawAtPoint:point];
    }
}

- (void) centerButtons{
//    [self.upButton setFrame:CGRectMake(self.scrollView.center.x + self.scrollView.contentOffset.x - (.5 * BTNBIG), self.scrollView.bounds.origin.y,self.upButton.bounds.size.width,self.upButton.bounds.size.height)];
//    [self.dnButton setFrame:CGRectMake(self.view.center.x + self.scrollView.contentOffset.x - (BTNBIG / 2), self.scrollView.bounds.size.height + self.scrollView.contentOffset.y - BTNSMALL, BTNBIG,BTNSMALL)];
//    [self.lfButton setFrame:CGRectMake(self.scrollView.bounds.origin.x, self.view.center.y + self.scrollView.contentOffset.y - (BTNBIG/ 2), BTNSMALL,BTNBIG)];
//    [self.rtButton setFrame:CGRectMake(self.view.bounds.size.width + self.scrollView.contentOffset.x - BTNSMALL, self.view.center.y + self.scrollView.contentOffset.y - (BTNBIG/2), BTNSMALL,BTNBIG)];
}

- (void)scrollViewDidEndZooming:(UIScrollView *)scrollView withView:(UIView *)view atScale:(float)scale{
    [self centerButtons];
}

- (void)scrollViewDidEndScrollingAnimation:(UIScrollView *) scrollView{
    [self centerButtons];
}

- (void)scrollViewDidEndDecelerating:(UIScrollView *) scrollView{
    [self centerButtons];
}

- (void)scrollViewDidScroll:(UIScrollView *) scrollView{
    [self centerButtons];
}

- (NSArray *)ivArray{
    if (!_ivArray) _ivArray = [[NSMutableArray alloc] init];
    return _ivArray;
}

- (PlacementBrain *)brain
{
    if (!_brain) _brain = [[PlacementBrain alloc] init];
    return _brain;
}

- (ImageModel *)images
{
    if (!_images) _images = [[ImageModel alloc] init];
    return _images;
}

- (UIView *)masterView{
    if (!_masterView) _masterView = [[UIView alloc] init];
    return _masterView;
}

-(UIActionSheet *)modeSheet{
    if (!_modeSheet) _modeSheet = [[UIActionSheet alloc] initWithTitle:@"Mode select" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Delete mode" otherButtonTitles:@"Edit mode", @"Add mode",@"Save and Exit", nil];
    return _modeSheet;
}


- (void)buttonsHidden:(BOOL) vis{
    self.upButton.hidden = vis;
    self.dnButton.hidden = vis;
    self.lfButton.hidden = vis;
    self.rtButton.hidden = vis;
    
}



//this is a double tap
- (IBAction)tapped:(id)sender {
    [self switchModeTo:NORMAL];
}

- (void)actionSheet:(UIActionSheet *)actionsheet clickedButtonAtIndex:(NSInteger)index{
    if (self.selectedPoint != nil)
        [self resetPointImage:self.selectedPoint];

    switch (index) {
        case 0:
            [self switchModeTo:DELETING];

            break;
            
        case 1:
            [self switchModeTo:MOVING];

            break;
            
        case 2:
            [self switchModeTo:NORMAL];

            break;
            
        case 3:
            [self.navigationController popViewControllerAnimated:YES];
            break;
            
        default:
            break;
    }
}

- (IBAction)longPress:(id)sender {
    //if (status == NORMAL)
        [self.modeSheet showInView:self.view];

}


-(void) centerAtX:(int)x andY:(int)y withZoomFactor:(double)zoomFactor{
    [self.scrollView setContentOffset:CGPointMake((zoomFactor * x) - (.5 * self.scrollView.bounds.size.width), (zoomFactor * y) - (.5 * self.scrollView.bounds.size.height)) animated:YES];
}



- (IBAction)singleTap:(id)sender {
    if (status == DELETING)
    {
        CGPoint loc = [self.singleTapRec locationInView:self.imageView];
        
        //find the closest point
        int closestIndex = 0;
        double closestDist = 999;

        if (self.brain.points.count > 3)
        {
            //loop thru all of the points
            NSEnumerator *e = [self.brain.points objectEnumerator];
            
            //skip  the normalization points
            [e nextObject];
            [e nextObject];
            [e nextObject];
            
            for (int i = 3; i < self.brain.points.count; i++) 
            {
                NSValue *obj = [e nextObject];
                double dist = sqrt(pow(loc.x - obj.CGPointValue.x,2.0) + pow(loc.y - obj.CGPointValue.y,2.0));
                if (dist < closestDist) {
                    closestDist = dist;
                    closestIndex = i;
                }
            }
            
            //prevent a random tap from removing a point
            if (closestDist < DISTTHRESHOLD){
                //remove from controller & view
                UIImageView *iv = [self.ivArray objectAtIndex:closestIndex];
                [iv removeFromSuperview];
                [self.ivArray removeObjectAtIndex:closestIndex];
                
                
                //remove from brain            
                [self.brain removePoint:[self.brain.points objectAtIndex:closestIndex]];

            }
        }
        

    }
    else if (status == MOVING) {
        CGPoint loc = [self.singleTapRec locationInView:self.masterView];
        
        //find the closest point
        int closestIndex = 0;
        double closestDist = 999;
        
        if (self.brain.points.count > 0)
        {
            //loop thru all of the points
            NSEnumerator *e = [self.brain.points objectEnumerator]; 
            for (int i = 0; i < self.brain.points.count; i++) 
            {
                NSValue *obj = [e nextObject];
                CGPoint point = obj.CGPointValue;
                point.x += self.imageView.center.x;
                point.y += self.imageView.center.y;
                point.y *= -1;
                double dist = sqrt(pow(loc.x - point.x,2.0) + pow(loc.y - point.y,2.0));
                if (dist < closestDist) {
                    closestDist = dist;
                    closestIndex = i;
                }
            
            }

            if (closestDist < EDITTHRESHOLD){
                double zoomFactor = 2.0;
                //remove from controller & view
                if (self.selectedPoint)
                    [self.selectedPoint setImage:self.images.circleImage];
                self.selectedPoint = [self.ivArray objectAtIndex:closestIndex];
                [self.selectedPoint setImage:self.images.editImage];
                [self.scrollView setZoomScale:zoomFactor];
                [self centerAtX:self.selectedPoint.center.x andY:self.selectedPoint.center.y withZoomFactor:zoomFactor];

                //[self.scrollView setContentOffset:CGPointMake(self.imageView.center.x - (self.scrollView.center.x - self.selectedPoint.center.x), self.imageView.center.y - (self.scrollView.center.y - self.selectedPoint.center.y)) animated:YES];
                selectedPointIndex = closestIndex;

                [self buttonsHidden:NO];
            }
            else {
                if (self.selectedPoint)
                    if (closestIndex == 0)
                        [self.selectedPoint setImage:self.images.normalPinkImage];
                    else if (closestIndex == 1)
                        [self.selectedPoint setImage:self.images.normalOrangeImage];
                    else if (closestIndex == 2)
                        [self.selectedPoint setImage:self.images.normalBlackImage];
                    else
                        [self.selectedPoint setImage:self.images.circleImage];
                [self.scrollView setZoomScale:1.0];
                [self.scrollView setContentOffset:CGPointMake(0, 0 )];
                selectedPointIndex = -1;
                [self buttonsHidden:YES];
            }
        }
    }else{
        CGPoint loc = [self.tapRecognizer locationInView:self.imageView];
        //add point to the detail view and coredata.
        CGPoint brainPoint = loc;
        brainPoint.x -= self.imageView.center.x;
        brainPoint.y -= self.imageView.center.y;
        brainPoint.y *= -1;
        [self.detailView addPointWithXValue:brainPoint.x andYValue:brainPoint.y];
        //draw them 
        [self drawAtPoint:loc];

    }

        
}


- (UITapGestureRecognizer *) tapRecognizer{
    if (!_tapRecognizer) _tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(tapped:)];
    _tapRecognizer.numberOfTapsRequired = 2;
    return _tapRecognizer;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)viewDidUnload {
    [self setImageView:nil];
    [self setTapRecognizer:nil];
    [self setLongPressRec:nil];
    [self setSingleTapRec:nil];
    [self setScrollView:nil];
    [self setMasterView:nil];
    [super viewDidUnload];
}
@end
