#!/usr/bin/env python3
"""Génère les icônes de ForthEditor (PNG 64x64, style plat arrondi). Dessin à 8x puis réduction."""
import math, sys, os
from PIL import Image, ImageDraw, ImageFont

S = 8
OUT = sys.argv[1] if len(sys.argv) > 1 else "icons"
os.makedirs(OUT, exist_ok=True)

# palette
BLUE = (59, 130, 246);   BLUE_D = (29, 78, 180);   BLUE_L = (191, 219, 254)
GREEN = (34, 197, 94);   GREEN_D = (21, 128, 61);  GREEN_L = (187, 247, 208)
RED = (239, 68, 68);     RED_D = (153, 27, 27);    RED_L = (254, 202, 202)
AMBER = (245, 158, 11);  AMBER_D = (180, 83, 9);   AMBER_L = (253, 230, 138)
PURPLE = (139, 92, 246); PURPLE_D = (91, 33, 182); PURPLE_L = (221, 214, 254)
TEAL = (20, 184, 166);   TEAL_D = (15, 118, 110)
SLATE = (100, 116, 139); SLATE_D = (30, 41, 59);   SLATE_L = (226, 232, 240)
WHITE = (255, 255, 255); PAPER = (248, 250, 252)
NONE = (0, 0, 0, 0)

try:
    FONT = "/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf"
    ImageFont.truetype(FONT, 10)
except Exception:
    FONT = None


class C:
    def __init__(s):
        s.im = Image.new("RGBA", (64 * S, 64 * S), NONE)
        s.d = ImageDraw.Draw(s.im)

    @staticmethod
    def _p(pts):
        return [(x * S, y * S) for x, y in pts]

    def poly(s, pts, fill, line=None, w=2):
        s.d.polygon(s._p(pts), fill=fill)
        if line:
            s.d.line(s._p(list(pts) + [pts[0]]), fill=line, width=int(w * S), joint="curve")

    def rrect(s, x0, y0, x1, y1, r, fill, line=None, w=2):
        s.d.rounded_rectangle([x0 * S, y0 * S, x1 * S, y1 * S], radius=r * S, fill=fill,
                              outline=line, width=int(w * S) if line else 0)

    def rect(s, x0, y0, x1, y1, fill, line=None, w=2):
        s.rrect(x0, y0, x1, y1, 0, fill, line, w)

    def ell(s, cx, cy, r, fill, line=None, w=2):
        s.d.ellipse([(cx - r) * S, (cy - r) * S, (cx + r) * S, (cy + r) * S], fill=fill,
                    outline=line, width=int(w * S) if line else 0)

    def line(s, pts, fill, w=3, round_ends=True):
        P = s._p(pts)
        s.d.line(P, fill=fill, width=int(w * S), joint="curve")
        if round_ends:
            for (x, y) in (P[0], P[-1]):
                s.d.ellipse([x - w * S / 2, y - w * S / 2, x + w * S / 2, y + w * S / 2], fill=fill)

    def arc(s, cx, cy, r, a0, a1, fill, w=5):
        s.d.arc([(cx - r) * S, (cy - r) * S, (cx + r) * S, (cy + r) * S], a0, a1, fill=fill, width=int(w * S))

    def text(s, x, y, t, size, fill):
        if not FONT:
            return
        f = ImageFont.truetype(FONT, int(size * S))
        s.d.text((x * S, y * S), t, font=f, fill=fill, anchor="mm")

    def save(s, name):
        s.im.resize((64, 64), Image.LANCZOS).save(os.path.join(OUT, name + ".png"))


# ---------- éléments réutilisables ----------
def page(c, x0=14, y0=6, x1=50, y1=58, fold=11, body=PAPER, line=SLATE, w=2.2):
    pts = [(x0, y0), (x1 - fold, y0), (x1, y0 + fold), (x1, y1), (x0, y1)]
    c.poly(pts, body, line, w)
    c.poly([(x1 - fold, y0), (x1 - fold, y0 + fold), (x1, y0 + fold)], SLATE_L, line, w * 0.8)


def text_lines(c, x0, x1, ys, col=SLATE, w=2.4):
    for y in ys:
        c.line([(x0, y), (x1, y)], col, w)


def folder(c, body=AMBER, dark=AMBER_D, light=AMBER_L, y=14):
    c.poly([(5, y + 4), (5, y), (12, y), (17, y + 5), (26, y + 5), (26, y + 8)], dark)
    c.rrect(5, y + 7, 59, 54, 4, dark)
    c.poly([(5, 54), (11, y + 15), (62, y + 15), (56, 54)], body, dark, 1.8)
    c.line([(14, y + 19), (56, y + 19)], light, 2)


def badge(c, cx, cy, r, col, dark, sym):
    c.ell(cx, cy, r, col, dark, 2)
    if sym == "plus":
        c.line([(cx - r * .5, cy), (cx + r * .5, cy)], WHITE, 3.2)
        c.line([(cx, cy - r * .5), (cx, cy + r * .5)], WHITE, 3.2)
    elif sym == "minus":
        c.line([(cx - r * .5, cy), (cx + r * .5, cy)], WHITE, 3.2)
    elif sym == "x":
        k = r * .42
        c.line([(cx - k, cy - k), (cx + k, cy + k)], WHITE, 3.2)
        c.line([(cx - k, cy + k), (cx + k, cy - k)], WHITE, 3.2)
    elif sym == "check":
        c.line([(cx - r * .5, cy), (cx - r * .1, cy + r * .4), (cx + r * .5, cy - r * .4)], WHITE, 3.2)
    elif sym == "pencil":
        pencil(c, cx, cy, r * 1.0, white=True)


def pencil(c, cx, cy, r, white=False):
    k = r * .55
    pts = [(cx + k, cy - k), (cx + k * 1.35, cy - k * .65), (cx - k * .35, cy + k * 1.05), (cx - k * 1.05, cy + k * 1.05),
           (cx - k * 1.05, cy + k * .35)]
    c.poly(pts, WHITE if white else AMBER, None)


def arrow(c, x0, y0, x1, y1, col, w=5, head=9):
    ang = math.atan2(y1 - y0, x1 - x0)
    bx, by = x1 - head * .8 * math.cos(ang), y1 - head * .8 * math.sin(ang)
    c.line([(x0, y0), (bx, by)], col, w)
    p1 = (x1, y1)
    p2 = (x1 - head * math.cos(ang) + head * .65 * math.sin(ang), y1 - head * math.sin(ang) - head * .65 * math.cos(ang))
    p3 = (x1 - head * math.cos(ang) - head * .65 * math.sin(ang), y1 - head * math.sin(ang) + head * .65 * math.cos(ang))
    c.poly([p1, p2, p3], col)


def circ_arrow(c, cx, cy, r, a0, a1, col, w=6, head=10, ccw=False):
    """arc de a0 à a1 (degrés, sens horaire écran) avec pointe à la fin (ou au début si ccw)."""
    c.arc(cx, cy, r, a0, a1, col, w)
    a = math.radians(a0 if ccw else a1)
    px, py = cx + r * math.cos(a), cy + r * math.sin(a)
    tx, ty = (-math.sin(a), math.cos(a)) if not ccw else (math.sin(a), -math.cos(a))
    nx, ny = math.cos(a), math.sin(a)
    tip = (px + tx * head * .9, py + ty * head * .9)
    b1 = (px + nx * head * .75, py + ny * head * .75)
    b2 = (px - nx * head * .75, py - ny * head * .75)
    c.poly([tip, b1, b2], col)


def magnifier(c, cx=26, cy=26, r=16, ring=BLUE, glass=BLUE_L, handle=(71, 85, 105), sx=None):
    c.line([(cx + r * .72, cy + r * .72), (cx + r * 1.65 + 3, cy + r * 1.65 + 3)], handle, 8)
    c.ell(cx, cy, r, glass, ring, 4.5)
    c.arc(cx, cy, r - 5, 200, 255, WHITE, 2)


def window(c, fill_zone=None, col=BLUE):
    c.rrect(6, 8, 58, 56, 5, PAPER, SLATE_D, 2.4)
    c.rect(6.5, 8.5, 57.5, 18, SLATE_L)
    c.rrect(6, 8, 58, 56, 5, None, SLATE_D, 2.4)
    for i, cc in enumerate((RED, AMBER, GREEN)):
        c.ell(13 + i * 6, 13, 1.8, cc)
    if fill_zone == "left":
        c.rect(8, 19.5, 24, 54, col)
    elif fill_zone == "right":
        c.rect(40, 19.5, 56, 54, col)
    elif fill_zone == "bottom":
        c.rect(8, 40, 56, 54, col)


def gear(c, cx, cy, ro, ri, hole, col, dark, teeth=8):
    pts = []
    n = teeth * 4
    for i in range(n):
        a = 2 * math.pi * i / n - math.pi / n
        r = ro if (i % 4) in (1, 2) else ri
        pts.append((cx + r * math.cos(a), cy + r * math.sin(a)))
    c.poly(pts, col, dark, 1.8)
    c.ell(cx, cy, hole, NONE)
    c.d.ellipse([(cx - hole) * S, (cy - hole) * S, (cx + hole) * S, (cy + hole) * S], fill=NONE)
    c.ell(cx, cy, hole, None, dark, 1.8)


ICONS = {}


def icon(f):
    ICONS[f.__name__.replace("ic_", "")] = f
    return f


# ---------- fichier ----------
@icon
def ic_new(c):
    page(c, 12, 6, 48, 58)
    text_lines(c, 19, 40, (26, 33, 40))
    badge(c, 45, 45, 13, GREEN, GREEN_D, "plus")


@icon
def ic_open(c):
    c.poly([(5, 18), (5, 12), (14, 12), (19, 17), (36, 17), (36, 22)], AMBER_D)
    c.rrect(5, 19, 46, 52, 3, AMBER_D)
    page(c, 14, 8, 44, 40, fold=8)
    c.poly([(4, 54), (13, 27), (62, 27), (53, 54)], AMBER, AMBER_D, 1.8)
    c.line([(15, 32), (56, 32)], AMBER_L, 2)


@icon
def ic_folder(c):
    folder(c)


@icon
def ic_openfolder(c):
    folder(c)
    # arbre de projet
    c.ell(21, 38, 2.6, SLATE_D); c.ell(21, 47, 2.6, SLATE_D)
    c.line([(21, 38), (21, 47)], SLATE_D, 1.8)
    c.line([(26, 38), (47, 38)], WHITE, 3)
    c.line([(26, 47), (41, 47)], WHITE, 3)


@icon
def ic_recent(c):
    c.ell(32, 32, 24, WHITE, BLUE, 5)
    c.line([(32, 32), (32, 17)], SLATE_D, 4)
    c.line([(32, 32), (43, 38)], SLATE_D, 4)
    c.ell(32, 32, 2.8, BLUE_D)
    c.arc(32, 32, 24, 200, 250, BLUE_D, 5)


@icon
def ic_save(c):
    c.poly([(8, 12), (8, 54), (56, 54), (56, 8), (14, 8)][::1], BLUE, BLUE_D, 2.4)
    c.rect(18, 8, 46, 26, PAPER, BLUE_D, 2)
    c.rect(37, 11, 42, 23, BLUE_D)
    c.rrect(15, 34, 49, 54, 2, SLATE_L, BLUE_D, 2)
    c.line([(21, 41), (43, 41)], SLATE, 2.2)
    c.line([(21, 47), (38, 47)], SLATE, 2.2)


@icon
def ic_saveas(c):
    c.poly([(6, 12), (6, 52), (46, 52), (46, 8), (12, 8)], BLUE, BLUE_D, 2.4)
    c.rect(14, 8, 38, 24, PAPER, BLUE_D, 2)
    c.rect(31, 11, 35, 21, BLUE_D)
    c.rrect(12, 32, 40, 52, 2, SLATE_L, BLUE_D, 2)
    c.ell(46, 44, 14, PURPLE, PURPLE_D, 2)
    pencil(c, 46, 44, 14, white=True)


@icon
def ic_print(c):
    c.poly([(18, 6), (42, 6), (48, 12), (48, 26), (18, 26)], PAPER, SLATE_D, 2.2)
    c.line([(23, 12), (38, 12)], SLATE, 2.2); c.line([(23, 18), (38, 18)], SLATE, 2.2)
    c.rrect(6, 24, 58, 46, 6, SLATE, SLATE_D, 2.4)
    c.ell(50, 31, 2.2, GREEN)
    c.poly([(16, 38), (48, 38), (50, 58), (14, 58)], WHITE, SLATE_D, 2.2)
    c.line([(21, 46), (43, 46)], SLATE, 2.2); c.line([(21, 52), (40, 52)], SLATE, 2.2)


@icon
def ic_close(c):
    c.rrect(6, 14, 58, 52, 6, PAPER, SLATE_D, 2.4)
    c.rrect(6, 14, 34, 26, 5, SLATE_L, SLATE_D, 2.2)
    badge(c, 41, 38, 12, RED, RED_D, "x")


@icon
def ic_delete(c):
    c.rrect(13, 17, 51, 21, 2, SLATE, SLATE_D, 2)
    c.rrect(24, 11, 40, 17, 2.5, SLATE, SLATE_D, 2)
    c.poly([(16, 22), (48, 22), (45, 57), (19, 57)], RED, RED_D, 2.4)
    for x in (26, 32, 38):
        c.line([(x, 28), (x + (0 if x == 32 else (-.8 if x < 32 else .8)), 51)], RED_L, 2.6)


@icon
def ic_quit(c):
    c.poly([(8, 8), (36, 8), (36, 56), (8, 56)], SLATE_L, SLATE_D, 2.4)
    c.poly([(8, 8), (30, 14), (30, 52), (8, 56)], AMBER, AMBER_D, 2)
    c.ell(25, 33, 2, AMBER_D)
    arrow(c, 30, 32, 58, 32, RED, 6, 12)


# ---------- édition ----------
@icon
def ic_undo(c):
    circ_arrow(c, 33, 38, 17, 180, 40, BLUE, 7, 14, ccw=True)


FLIPPED = {"redo": "undo"}


@icon
def ic_cut(c):
    c.line([(17, 47), (45, 6)], (148, 163, 184), 6)
    c.line([(47, 47), (19, 6)], (71, 85, 105), 6)
    c.ell(32, 27, 3, AMBER, SLATE_D, 1.4)
    for x in (16, 48):
        c.ell(x, 50, 8.5, RED, RED_D, 2)
        c.ell(x, 50, 3.4, NONE)
        c.d.ellipse([(x - 3.4) * S, (50 - 3.4) * S, (x + 3.4) * S, (50 + 3.4) * S], fill=NONE)


@icon
def ic_copy(c):
    page(c, 8, 6, 40, 46, fold=9, body=BLUE_L, line=BLUE_D)
    page(c, 22, 18, 56, 58, fold=9, body=PAPER, line=BLUE_D)
    text_lines(c, 29, 49, (36, 43, 50), BLUE, 2.4)


@icon
def ic_paste(c):
    c.rrect(8, 10, 46, 58, 5, AMBER, AMBER_D, 2.4)
    c.rrect(18, 5, 36, 15, 3, SLATE, SLATE_D, 2)
    page(c, 24, 22, 56, 60, fold=9)
    text_lines(c, 31, 49, (38, 45, 52), SLATE, 2.4)


@icon
def ic_selectall(c):
    c.d.rectangle([8 * S, 10 * S, 56 * S, 54 * S], outline=BLUE, width=int(3 * S))
    for i in range(0, 48, 12):
        c.rect(8 + i, 9, 8 + i + 4, 11.5, PAPER)
        c.rect(8 + i, 52.5, 8 + i + 4, 55, PAPER)
    text_lines(c, 15, 49, (24, 32, 40), SLATE, 3)
    c.rect(15, 21, 36, 27, BLUE_L); text_lines(c, 15, 36, (24,), SLATE_D, 3)


@icon
def ic_find(c):
    magnifier(c)


@icon
def ic_findnext(c):
    magnifier(c, 24, 24, 15)
    c.poly([(36, 50), (58, 50), (47, 62)], GREEN, GREEN_D, 1.6)


@icon
def ic_findprev(c):
    magnifier(c, 24, 28, 15)
    c.poly([(36, 14), (58, 14), (47, 2)][::1], GREEN, GREEN_D, 1.6)


@icon
def ic_replace(c):
    arrow(c, 10, 22, 54, 22, BLUE, 7, 13)
    arrow(c, 54, 42, 10, 42, AMBER, 7, 13)
    c.rrect(8, 28, 56, 36, 2, SLATE_L)


@icon
def ic_goto(c):
    for y in (12, 22, 42, 52):
        c.line([(18, y), (54, y)], SLATE, 3.4)
    c.rrect(14, 28, 58, 38, 3, AMBER_L, AMBER_D, 2)
    arrow(c, 4, 33, 18, 33, GREEN, 5, 10)
    c.line([(23, 33), (48, 33)], AMBER_D, 3)


@icon
def ic_reindent(c):
    c.line([(24, 12), (56, 12)], SLATE, 3.4)
    c.line([(32, 24), (56, 24)], PURPLE, 3.4)
    c.line([(40, 36), (56, 36)], PURPLE, 3.4)
    c.line([(24, 48), (56, 48)], SLATE, 3.4)
    arrow(c, 5, 30, 20, 30, GREEN, 5, 10)
    c.line([(6, 14), (6, 52)], SLATE_L, 2.4)


# ---------- affichage ----------
@icon
def ic_refresh(c):
    c.arc(32, 32, 19, 20, 160, TEAL, 7)
    c.arc(32, 32, 19, 200, 340, TEAL, 7)
    a = math.radians(160); px, py = 32 + 19 * math.cos(a), 32 + 19 * math.sin(a)
    c.poly([(px - 10, py - 4), (px + 8, py - 8), (px + 1, py + 10)], TEAL_D)
    a = math.radians(340); px, py = 32 + 19 * math.cos(a), 32 + 19 * math.sin(a)
    c.poly([(px + 10, py + 4), (px - 8, py + 8), (px - 1, py - 10)], TEAL_D)


@icon
def ic_syntax_reload(c):
    page(c, 8, 6, 40, 52, fold=9)
    for y, col in ((20, RED), (27, BLUE), (34, GREEN), (41, AMBER)):
        c.line([(14, y), (14 + [12, 17, 9, 14][(y - 20) // 7], y)], col, 3)
    c.arc(44, 44, 11, 20, 160, TEAL, 5)
    c.arc(44, 44, 11, 200, 340, TEAL, 5)
    c.poly([(33, 46), (41, 49), (33, 53)], TEAL_D)
    c.poly([(55, 42), (47, 39), (55, 35)], TEAL_D)


@icon
def ic_syntax_edit(c):
    page(c, 8, 6, 42, 56, fold=10)
    for y, col in ((20, RED), (27, BLUE), (34, GREEN)):
        c.line([(14, y), (14 + [14, 19, 10][(y - 20) // 7], y)], col, 3)
    c.line([(26, 52), (54, 24)], AMBER, 9)
    c.poly([(21, 58), (23, 48), (31, 56)], SLATE_D)
    c.line([(50, 20), (58, 28)], RED, 8)


@icon
def ic_theme(c):
    c.ell(32, 32, 24, WHITE, SLATE_D, 3)
    c.d.pieslice([8 * S, 8 * S, 56 * S, 56 * S], 90, 270, fill=SLATE_D)
    c.ell(32, 32, 24, None, SLATE_D, 3)


@icon
def ic_theme_dark(c):
    c.ell(29, 34, 23, PURPLE, PURPLE_D, 2)
    c.d.ellipse([24 * S, 4 * S, 62 * S, 42 * S], fill=NONE)
    c.ell(48, 14, 2.2, AMBER_L); c.ell(54, 28, 1.6, AMBER_L); c.ell(46, 46, 2, AMBER_L)


@icon
def ic_theme_light(c):
    for i in range(8):
        a = math.pi * i / 4
        c.line([(32 + 17 * math.cos(a), 32 + 17 * math.sin(a)), (32 + 26 * math.cos(a), 32 + 26 * math.sin(a))], AMBER, 4)
    c.ell(32, 32, 13, AMBER_L, AMBER_D, 3)


def zoom(c, sym):
    magnifier(c, 26, 26, 17)
    c.line([(18, 26), (34, 26)], BLUE_D, 4)
    if sym == "+":
        c.line([(26, 18), (26, 34)], BLUE_D, 4)
    elif sym == "1":
        c.line([(26, 18), (26, 34)], BLUE_D, 4)
        c.line([(20, 20), (26, 16)], BLUE_D, 3)


@icon
def ic_zoomin(c):
    zoom(c, "+")


@icon
def ic_zoomout(c):
    zoom(c, "-")


@icon
def ic_zoomreset(c):
    magnifier(c, 26, 26, 17)
    c.text(26, 27, "1:1", 9.5, BLUE_D)


@icon
def ic_pane_project(c):
    window(c, "left", BLUE)
    text_lines(c, 10, 22, (28, 36, 44), WHITE, 2.4)


@icon
def ic_pane_outline(c):
    window(c, "right", GREEN)
    text_lines(c, 42, 54, (28, 36, 44), WHITE, 2.4)
    text_lines(c, 11, 34, (28, 36, 44), SLATE, 2.4)


@icon
def ic_pane_output(c):
    window(c, "bottom", PURPLE)
    text_lines(c, 11, 34, (26,), SLATE, 2.4)
    c.line([(11, 45), (20, 45)], WHITE, 2.6); c.line([(23, 45), (41, 45)], PURPLE_L, 2.6)


@icon
def ic_fold(c):
    c.rrect(10, 10, 54, 54, 6, AMBER_L, AMBER_D, 3)
    c.line([(20, 32), (44, 32)], AMBER_D, 5)


@icon
def ic_foldall(c):
    c.line([(14, 8), (32, 24), (50, 8)], AMBER_D, 6)
    c.line([(14, 56), (32, 40), (50, 56)], AMBER_D, 6)
    c.line([(12, 32), (52, 32)], AMBER, 5)


@icon
def ic_unfoldall(c):
    c.line([(14, 24), (32, 8), (50, 24)], AMBER_D, 6)
    c.line([(14, 40), (32, 56), (50, 40)], AMBER_D, 6)
    c.line([(12, 32), (52, 32)], SLATE_L, 4)


@icon
def ic_foldtoggle(c):
    c.rrect(10, 10, 54, 54, 6, AMBER_L, AMBER_D, 3)
    c.line([(20, 32), (44, 32)], AMBER_D, 5)
    c.line([(32, 20), (32, 44)], AMBER_D, 5)
    c.rect(29, 20, 35, 44, AMBER_L)
    c.line([(20, 32), (44, 32)], AMBER_D, 5)
    c.poly([(26, 18), (38, 18), (32, 12)], AMBER_D)
    c.poly([(26, 46), (38, 46), (32, 52)], AMBER_D)


@icon
def ic_session(c):
    ic_recent(c)


# ---------- exécution ----------
@icon
def ic_run(c):
    c.ell(32, 32, 27, GREEN, GREEN_D, 3)
    c.poly([(25, 19), (25, 45), (47, 32)], WHITE)


@icon
def ic_stop(c):
    c.ell(32, 32, 27, RED, RED_D, 3)
    c.rrect(21, 21, 43, 43, 3, WHITE)


@icon
def ic_clear(c):
    c.poly([(10, 40), (30, 12), (58, 32), (38, 60)][::1], PURPLE_L, PURPLE_D, 2.4)
    c.poly([(10, 40), (21, 25), (49, 46), (38, 60)][::1], PURPLE, PURPLE_D, 2.4)
    c.line([(4, 60), (22, 60)], SLATE, 3)
    c.line([(40, 60), (58, 60)], SLATE, 3)


# ---------- outils ----------
@icon
def ic_terminal(c):
    c.rrect(5, 9, 59, 55, 6, SLATE_D, SLATE, 2.4)
    c.rect(6.5, 10.5, 57.5, 18, SLATE)
    for i, cc in enumerate((RED, AMBER, GREEN)):
        c.ell(12 + i * 5.5, 14, 1.7, cc)
    c.line([(14, 28), (23, 34), (14, 40)], GREEN, 4)
    c.line([(28, 43), (44, 43)], AMBER, 4)


@icon
def ic_settings(c):
    gear(c, 32, 32, 27, 20, 8, SLATE, SLATE_D)
    c.ell(32, 32, 6, BLUE)


@icon
def ic_autocomplete(c):
    c.rrect(6, 8, 50, 22, 3, PAPER, SLATE_D, 2.2)
    c.line([(11, 15), (24, 15)], SLATE, 3)
    c.line([(26, 11), (26, 19)], BLUE, 2.4)
    c.rrect(6, 25, 50, 56, 3, PAPER, SLATE_D, 2.2)
    c.rect(8, 28, 48, 36, BLUE)
    c.line([(12, 32), (30, 32)], WHITE, 2.6)
    c.line([(12, 42), (34, 42)], SLATE, 2.6); c.line([(12, 49), (28, 49)], SLATE, 2.6)
    for (x, y, r) in ((54, 14, 7), (46, 4, 4)):
        c.poly([(x, y - r), (x + r * .3, y - r * .3), (x + r, y), (x + r * .3, y + r * .3), (x, y + r),
                (x - r * .3, y + r * .3), (x - r, y), (x - r * .3, y - r * .3)], AMBER, AMBER_D, 1)


# ---------- aide ----------
@icon
def ic_help(c):
    c.poly([(5, 14), (30, 18), (30, 56), (5, 52)], BLUE, BLUE_D, 2.2)
    c.poly([(59, 14), (34, 18), (34, 56), (59, 52)], BLUE_L, BLUE_D, 2.2)
    c.poly([(30, 18), (34, 18), (34, 56), (30, 56)], BLUE_D)
    text_lines(c, 38, 54, (26, 33, 40), SLATE, 2.2)
    c.ell(17, 33, 7, WHITE)
    c.text(17, 34, "?", 11, BLUE_D)


@icon
def ic_pdf(c):
    page(c, 10, 4, 50, 60, fold=11)
    c.rrect(5, 30, 44, 46, 3, RED, RED_D, 2)
    c.text(24.5, 38.5, "PDF", 10.5, WHITE)
    text_lines(c, 17, 34, (14, 21), SLATE, 2.4)


@icon
def ic_pdfopen(c):
    page(c, 10, 4, 48, 60, fold=11)
    c.rrect(5, 30, 42, 46, 3, RED, RED_D, 2)
    c.text(23.5, 38.5, "PDF", 10.5, WHITE)
    arrow(c, 50, 58, 50, 36, GREEN, 6, 11)
    text_lines(c, 17, 32, (14, 21), SLATE, 2.4)


@icon
def ic_pdffolder(c):
    folder(c)
    c.rrect(20, 28, 50, 46, 3, RED, RED_D, 2)
    c.text(35, 37.5, "PDF", 9.5, WHITE)


@icon
def ic_about(c):
    c.ell(32, 32, 27, BLUE, BLUE_D, 3)
    c.ell(32, 18, 4, WHITE)
    c.rrect(28, 26, 36, 47, 3, WHITE)


if __name__ == "__main__":
    from PIL import ImageOps
    for name, fn in ICONS.items():
        c = C()
        fn(c)
        c.save(name)
    for name, src in FLIPPED.items():
        im = Image.open(os.path.join(OUT, src + ".png"))
        ImageOps.mirror(im).save(os.path.join(OUT, name + ".png"))
    print(len(ICONS), "icônes ->", OUT)
