//
//  PlacementBrain.h
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//


//#import "ImageRecController.h"
#import <Foundation/Foundation.h>
//#import "ImageRecController.h"


@interface PlacementBrain : NSObject

@property (nonatomic, strong) UIImage *targetImage;
@property (nonatomic, strong) UIImage *circleImage;
@property (nonatomic, strong) UIImage *editImage;
@property (nonatomic, strong) NSMutableArray *points;
@property (nonatomic, strong) NSArray *animationArray;

- (void)addPointatX:(CGFloat)x andY:(CGFloat)y;
- (void)removePointatX:(int)x andY:(int)y;
-(void) removePoint:(NSValue *)p;
-(void) replacePointAtIndex:(int)i withPoint:(CGPoint) point;
-(Point*) processImage;

@end
